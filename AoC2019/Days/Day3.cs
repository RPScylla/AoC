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
    class Day3 : IDay
    {
        private const int AOC_DAY = 3;

        private readonly string[] _input;

        public Day3()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            Dictionary<string, int> map = new Dictionary<string, int>();
            HashSet<string> intersects = new HashSet<string>();

            string[] wireA = _input[0].Split(',');
            string[] wireB = _input[1].Split(',');

            TraceLine(wireA, map, intersects, 0);
            TraceLine(wireB, map, intersects, 1);

            int minDistance = int.MaxValue;
            int minSteps = int.MaxValue;
            foreach (string intersect in intersects)
            {
                string[] pos = intersect.Split('=');
                int x = int.Parse(pos[0]);
                int y = int.Parse(pos[1]);

                int distance = Math.Abs(x) + Math.Abs(y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }

                int steps = map[intersect + "=0"] + map[intersect + "=1"];
                if(steps < minSteps)
                {
                    minSteps = steps;
                }
            }

            answer.PartA = minDistance;
            answer.PartB = minSteps;

            // Return the answer.
            return answer;
        }

        private void TraceLine(string[] commands, Dictionary<string, int> map, HashSet<string> intersects, int linenr)
        {
            int x = 0;
            int y = 0;
            int dx = 0;
            int dy = 0;
            int steps = 0;
            foreach (string command in commands)
            {
                int length = int.Parse(command.Substring(1));
                switch (command[0])
                {
                    case 'L':
                        dx = -1;
                        dy = 0;
                        break;
                    case 'U':
                        dx = 0;
                        dy = 1;
                        break;
                    case 'R':
                        dx = 1;
                        dy = 0;
                        break;
                    case 'D':
                        dx = 0;
                        dy = -1;
                        break;
                }

                for (int i = 0; i < length; i++)
                {
                    x += dx;
                    y += dy;

                    if (!map.ContainsKey($"{x}={y}={linenr}"))
                    {
                        map.Add($"{x}={y}={linenr}", ++steps);
                    }
                    else
                    {
                        steps++;
                    }

                    for (int j = linenr - 1; j >= 0; j--)
                    {
                        if (map.ContainsKey($"{x}={y}={j}"))
                        {
                            intersects.Add($"{x}={y}");
                        }
                    }
                }
            }
        }
    }
}
