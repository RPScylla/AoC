using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RPSonline.AoC.E2018.Days
{
    class Day6 : IDay
    {
        private readonly string[] _input;
        private readonly Regex _regex;

        public Day6()
        {
            _input = File.ReadAllLines("Inputs/Day6.txt");
            _regex = new Regex(@"(\d+),\s*(\d+)", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            List<Cord> cords = new List<Cord>();
            Dictionary<int, int> occurence = new Dictionary<int, int>();

            int sizeX = 0;
            int sizeY = 0;
            int id = 1;
            foreach (string line in _input)
            {
                Match match = _regex.Match(line);
                Cord cord = new Cord(match, id++);
                cords.Add(cord);
                occurence.Add(cord.Id, 0);
                sizeX = Math.Max(sizeX, cord.X);
                sizeY = Math.Max(sizeY, cord.Y);
            }

            int regionFitAll = 0;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Cord closest = GetClosest(x, y, cords);
                    bool fitAll = CheckIfCordsInDistance(x, y, cords, 10000);

                    if (fitAll)
                    {
                        regionFitAll++;
                    }

                    if (closest.Id == 0) { continue; }

                    occurence[closest.Id]++;
                    if (x == 0 || x == sizeX - 1 || y == 0 || y == sizeY - 1)
                    {
                        closest.Infinite = true;
                    }
                }
            }

            answer.PartB = regionFitAll;

            int highest = 0;
            Cord currentHighest;
            foreach(Cord cord in cords.Where(c => !c.Infinite))
            {
                if(occurence[cord.Id] > highest)
                {
                    currentHighest = cord;
                    highest = occurence[cord.Id];
                }
            }

            answer.PartA = highest;

            return answer;
        }

        private Cord GetClosest(int x, int y, IReadOnlyList<Cord> cords)
        {
            int lowest = int.MaxValue;
            Cord closest = new Cord();

            foreach (Cord cord in cords)
            {
                int heightDif = Math.Abs(cord.X - x);
                int widthDif = Math.Abs(cord.Y - y);
                int manhattanDif = heightDif + widthDif;

                if (manhattanDif < lowest)
                {
                    lowest = manhattanDif;
                    closest = cord;
                }
                else if (manhattanDif == lowest)
                {
                    closest = new Cord();
                }
            }
            return closest;
        }

        private bool CheckIfCordsInDistance(int x, int y, List<Cord> cords, int distance)
        {
            int sumDif = 0;
            foreach(Cord cord in cords)
            {
                int heightDif = Math.Abs(cord.X - x);
                int widthDif = Math.Abs(cord.Y - y);
                int manhattanDif = heightDif + widthDif;

                sumDif += manhattanDif;
                if (sumDif > distance)
                {
                    return false;
                }
            }
            return true;
        }


        class Cord
        {
            public int Id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool Infinite { get; set; }

            public Cord()
            {
                Id = 0;
            }
            public Cord(Match match, int id)
            {
                Id = id;
                Infinite = false;
                X = int.Parse(match.Groups[1].Value);
                Y = int.Parse(match.Groups[2].Value);
            }
        }
    }
}
