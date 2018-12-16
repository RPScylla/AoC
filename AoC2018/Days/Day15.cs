using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day15 : IDay
    {
        private const int AOC_DAY = 15;

        private readonly string[] _input;
        private readonly List<Point> _deltas;
        public Day15()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _deltas = new List<Point>() { new Point(0, -1), new Point(-1, 0), new Point(1, 0), new Point(0, 1) };
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            bool[,] map = new bool[_input.Length, _input[0].Length];
            List<Entity> initialEntities = new List<Entity>();

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    char current = _input[y][x];
                    map[y, x] = current != '#';
                    EntityType type = current.Equals('E') ? EntityType.Elve : (current.Equals('G') ? EntityType.Goblin : EntityType.None);
                    if (type != EntityType.None)
                    {
                        initialEntities.Add(new Entity(x, y, type));
                    }
                }
            }

            int elvesAttackPower = 1;
            int initalElves = initialEntities.Count(e => e.EType == EntityType.Elve);

            while (answer.PartA is null || answer.PartB is null)
            {
                int rounds = -1;
                List<Entity> entities = new List<Entity>();
                foreach (Entity e in initialEntities)
                {
                    entities.Add(new Entity(e.X, e.Y, e.EType, elvesAttackPower));
                }
                while (entities.Count(e => e.EType == EntityType.Elve) > 0 && entities.Count(e => e.EType == EntityType.Goblin) > 0)
                {
                    entities = entities.OrderBy(e => e.Y).ThenBy(e => e.X).ToList();
                    for (int i = 0; i < entities.Count(); i++)
                    {
                        Entity current = entities[i];
                        IEnumerable<Entity> targets = entities.Where(e => e.EType != current.EType);

                        if (!targets.Any(t => Adjacent(current, t)))
                        {
                            Move(current, map, entities, targets);
                        }

                        Entity attackTarget = targets.Where(e => Adjacent(current, e)).OrderBy(e => e.HP).ThenBy(e => e.Y).ThenBy(e => e.X).FirstOrDefault();
                        if (attackTarget is null)
                        {
                            continue;
                        }

                        attackTarget.HP -= current.AttackPower;
                        if (attackTarget.HP <= 0)
                        {
                            int removeIndex = entities.IndexOf(attackTarget);
                            entities.Remove(attackTarget);
                            if (removeIndex < i)
                            {
                                i--;
                            }
                        }

                    }
                    rounds++;

                    if (elvesAttackPower != 3 && initalElves != entities.Count(e => e.EType == EntityType.Elve))
                    {
                        break;
                    }
                }

                if (elvesAttackPower == 3)
                {
                    answer.PartA = rounds * entities.Sum(e => e.HP);
                }
                if (answer.PartB is null && initalElves == entities.Count(e => e.EType == EntityType.Elve))
                {
                    answer.PartB = rounds * entities.Sum(e => e.HP);
                }

                elvesAttackPower++;
            }

            return answer;
        }

        private void Move(Entity current, bool[,] map, List<Entity> entities, IEnumerable<Entity> targets)
        {
            HashSet<Point> freeAdjacents = new HashSet<Point>();
            foreach (Entity target in targets)
            {
                foreach (Point delta in _deltas)
                {
                    if (Walkable(target.X + delta.X, target.Y + delta.Y, map, entities))
                    {
                        freeAdjacents.Add(new Point(target.X + delta.X, target.Y + delta.Y));
                    }
                }

            }

            Queue<Point> queue = new Queue<Point>();
            Dictionary<Point, Point> visitedLinks = new Dictionary<Point, Point>();

            queue.Enqueue(new Point(current.X, current.Y));
            while (queue.Count > 0)
            {
                Point currentP = queue.Dequeue();
                foreach (Point delta in _deltas)
                {
                    Point nextP = new Point(currentP.X + delta.X, currentP.Y + delta.Y);
                    if (Walkable(nextP.X, nextP.Y, map, entities) && !visitedLinks.ContainsKey(nextP))
                    {
                        queue.Enqueue(nextP);
                        visitedLinks.Add(nextP, currentP);
                    }
                }
            }

            List<Point> bestPath = null;

            foreach (Point freeAdjacent in freeAdjacents)
            {
                List<Point> path = GetPath(current, freeAdjacent.X, freeAdjacent.Y, visitedLinks);
                if (bestPath is null)
                {
                    bestPath = path;
                }
                else if (path != null && (path.Count < bestPath.Count ||
                     (path.Count == bestPath.Count && path.Last().Y < bestPath.Last().Y) ||
                     (path.Count == bestPath.Count && path.Last().Y == bestPath.Last().Y && path.Last().X < bestPath.Last().X)))
                {
                    bestPath = path;
                }
            }

            if (bestPath != null)
            {
                current.X = bestPath[0].X;
                current.Y = bestPath[0].Y;
            }
        }

        private List<Point> GetPath(Entity current, int toX, int toY, Dictionary<Point, Point> visitedLinks)
        {
            Point currentP = new Point(toX, toY);

            if (!visitedLinks.ContainsKey(currentP))
            {
                return null;
            }

            List<Point> path = new List<Point>();
            while (currentP.X != current.X || currentP.Y != current.Y)
            {
                path.Add(new Point(currentP.X, currentP.Y));
                currentP = visitedLinks[currentP];
            }

            path.Reverse();
            return path;
        }

        private bool Adjacent(Entity a, Entity b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) == 1;
        }

        private bool Walkable(int x, int y, bool[,] map, List<Entity> entities)
        {
            return map[y, x] && !entities.Any(e => e.X == x && e.Y == y);
        }


        class Entity
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int HP { get; set; }
            public int AttackPower { get; set; }
            public EntityType EType { get; set; }

            public Entity(int x, int y, EntityType type)
            {
                X = x;
                Y = y;
                EType = type;
                AttackPower = 3;
                HP = 200;
            }

            public Entity(int x, int y, EntityType type, int elvesAttackPower) :
                this(x, y, type)
            {
                if (type == EntityType.Elve)
                {
                    AttackPower = elvesAttackPower;
                }
            }
        }
        enum EntityType
        {
            None,
            Elve,
            Goblin
        }
        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }


        }

    }
}
