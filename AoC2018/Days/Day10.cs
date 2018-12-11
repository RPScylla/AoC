using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day10 : IDay
    {
        private const int AOC_DAY = 10;

        private readonly string[] _input;
        private readonly Regex _regex;
        private const int CHARSIZE = 10;

        public Day10()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex(@"position=<\s*(-?\d+),\s*(-?\d+)> velocity=<\s*(-?\d+),\s*(-?\d+)>", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            List<Star> stars = new List<Star>();

            foreach (string line in _input)
            {
                Match match = _regex.Match(line);
                Star star = new Star(match);
                stars.Add(star);
            }

            int seconds = 0;
            while (true)
            {
                StarCordsDiff diff = GetBiggestDif(stars);

                if (diff.DifY + 1 == CHARSIZE)
                {
                    Draw(stars, diff);
                    Console.SetCursorPosition(0, CHARSIZE);
                    break;
                }
                seconds++;
                stars.ForEach(star => star.Tick());
            }

            Console.SetCursorPosition(0, CHARSIZE + 1);

            answer.PartA = "^";
            answer.PartB = seconds;
            return answer;
        }

        private void Draw(List<Star> stars, StarCordsDiff diff)
        {
            Console.Clear();
            int consolex = Math.Max(Console.WindowWidth, diff.DifX + 1);
            int consoleY = Math.Max(Console.WindowHeight, diff.DifY + 1);
            Console.SetBufferSize(consolex, consoleY);
            foreach (Star star in stars)
            {
                Console.SetCursorPosition(Math.Abs(star.X - diff.MinX), Math.Abs(star.Y - diff.MinY));
                Console.Write("#");
            }
        }

        private StarCordsDiff GetBiggestDif(List<Star> stars)
        {
            Star first = stars.First();
            int maxX = first.X;
            int maxY = first.Y;
            int minX = first.X;
            int minY = first.Y;

            foreach (Star star in stars)
            {
                maxX = Math.Max(maxX, star.X);
                minX = Math.Min(minX, star.X);
                maxY = Math.Max(maxY, star.Y);
                minY = Math.Min(minY, star.Y);
            }

            return new StarCordsDiff(Math.Abs(maxX - minX), Math.Abs(maxY - minY), minX, minY);
        }

        struct StarCordsDiff
        {
            public int MinX { get; set; }
            public int MinY { get; set; }

            public int DifX { get; set; }
            public int DifY { get; set; }

            public StarCordsDiff(int difX, int difY, int minX, int minY)
            {
                DifX = difX;
                DifY = difY;
                MinX = minX;
                MinY = minY;
            }
        }

        class Star
        {
            private readonly int _velocityX;
            private readonly int _velocityY;

            public int X { get; private set; }
            public int Y { get; private set; }

            public Star(Match match)
            {
                X = int.Parse(match.Groups[1].Value);
                Y = int.Parse(match.Groups[2].Value);
                _velocityX = int.Parse(match.Groups[3].Value);
                _velocityY = int.Parse(match.Groups[4].Value);
            }

            public void Tick()
            {
                X += _velocityX;
                Y += _velocityY;
            }
        }
    }
}
