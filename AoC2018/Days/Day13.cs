using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day13 : IDay
    {
        private const int AOC_DAY = 13;

        private readonly string[] _input;

        public Day13()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            char[,] track = GenerateTrack(_input, out List<Minecart> minecarts);

            while (minecarts.Count() > 1)
            {
                foreach (Minecart m in minecarts.OrderBy(m => m.Y).ThenBy(m => m.X))
                {
                    m.Tick(track);
                    Minecart crashedCar = minecarts.FirstOrDefault(mm => mm != m && mm.X == m.X && mm.Y == m.Y);
                    if (crashedCar != null)
                    {
                        crashedCar.Crashed = true;
                        m.Crashed = true;
                        if (answer.PartA is null)
                        {
                            answer.PartA = $"{m.X},{m.Y}";
                        }
                    }
                }
                minecarts.RemoveAll(m => m.Crashed);
            }

            Minecart notCrashed = minecarts.First();
            answer.PartB = $"{notCrashed.X},{notCrashed.Y}";

            return answer;
        }

        private char[,] GenerateTrack(string[] input, out List<Minecart> minecarts)
        {
            char[,] track = new char[input.Length, input[0].Length];
            minecarts = new List<Minecart>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char c = input[y][x];
                    track[y, x] = c;
                    Direction? createCartInDirection = null;

                    if (c.Equals('<'))
                    {
                        createCartInDirection = Direction.Left;
                        track[y, x] = '-';
                    }
                    else if (c.Equals('>'))
                    {
                        createCartInDirection = Direction.Right;
                        track[y, x] = '-';
                    }
                    else if (c.Equals('^'))
                    {
                        createCartInDirection = Direction.Up;
                        track[y, x] = '|';
                    }
                    else if (c.Equals('v'))
                    {
                        createCartInDirection = Direction.Down;
                        track[y, x] = '|';
                    }

                    if (createCartInDirection.HasValue)
                    {
                        Minecart minecart = new Minecart(createCartInDirection.Value, x, y);
                        minecarts.Add(minecart);
                    }
                }
            }
            return track;
        }


        class Minecart
        {
            public Direction Direction { get; set; }
            public int TurnNumber { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool Crashed { get; set; }

            public Minecart(Direction direction, int x, int y)
            {
                Direction = direction;
                X = x;
                Y = y;
                TurnNumber = 0;
                Crashed = false;
            }

            public void Tick(char[,] track)
            {
                switch (Direction)
                {
                    case Direction.Left: X--; break;
                    case Direction.Up: Y--; break;
                    case Direction.Right: X++; break;
                    case Direction.Down: Y++; break;
                }

                switch (track[Y, X])
                {
                    case '/':
                        Direction = LeftTurn(Direction);
                        break;
                    case '\\':
                        Direction = RightTurn(Direction);
                        break;
                    case '+':
                        if (TurnNumber == 0)
                        {
                            Direction = (Direction)(((int)Direction + 3) % 4);
                        }
                        else if (TurnNumber == 2)
                        {
                            Direction = (Direction)(((int)Direction + 1) % 4);
                        }
                        TurnNumber = (TurnNumber + 1) % 3;
                        break;
                }
            }

            private Direction LeftTurn(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Left: return Direction.Down;
                    case Direction.Up: return Direction.Right;
                    case Direction.Right: return Direction.Up;
                    case Direction.Down: return Direction.Left;
                }
                throw new NotImplementedException();
            }

            private Direction RightTurn(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Left: return Direction.Up;
                    case Direction.Up: return Direction.Left;
                    case Direction.Right: return Direction.Down;
                    case Direction.Down: return Direction.Right;
                }
                throw new NotImplementedException();
            }
        }

        enum Direction
        {
            Left,
            Up,
            Right,
            Down
        }
    }
}
