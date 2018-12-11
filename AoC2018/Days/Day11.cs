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
    class Day11 : IDay
    {
        private const int AOC_DAY = 11;

        private readonly uint _input;

        public Day11()
        {
            string input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _input = uint.Parse(input);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            int size = 300;
            long[,] grid = new long[size, size];
            long[,] summedGrid = new long[size, size];
            for (uint x = 0; x < size; x++)
            {
                for (uint y = 0; y < size; y++)
                {
                    long rackID = (x + 1) + 10;
                    long powerLevel = rackID * (y + 1);
                    powerLevel += _input;
                    powerLevel *= rackID;
                    powerLevel = (powerLevel / 100) % 10;
                    powerLevel -= 5;

                    long temp = 0;
                    if (y > 0)
                    {
                        temp += summedGrid[x, y - 1];
                    }
                    if (x > 0)
                    {
                        temp += summedGrid[x - 1, y];
                    }
                    if (x > 0 && y > 0)
                    {
                        temp -= summedGrid[x - 1, y - 1];
                    }
                    summedGrid[x, y] = temp + powerLevel;
                }
            }

            long totalBiggestValue = long.MinValue;
            Point biggestTopLeft = new Point();
            int biggestGridFieldSize = 0;

            for (int i = 1; i <= size; i++)
            {
                long currentValue = FindBiggestSummedArea(summedGrid, (uint)i, out Point currentTopLeft);

                if (i == 3)
                {
                    answer.PartA = $"{currentTopLeft.X},{currentTopLeft.Y}";
                }

                if (currentValue > totalBiggestValue)
                {
                    totalBiggestValue = currentValue;
                    biggestTopLeft = currentTopLeft;
                    biggestGridFieldSize = i;
                }
            }

            answer.PartB = $"{biggestTopLeft.X},{biggestTopLeft.Y},{biggestGridFieldSize}";

            return answer;
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

        private long FindBiggestSummedArea(long[,] summedAreaGrid, uint fieldSize, out Point cords)
        {
            long biggest = long.MinValue;
            Point biggestTopLeft = new Point();

            for (int x = 0; x < summedAreaGrid.GetLength(0) - (fieldSize - 1); x++)
            {
                for (int y = 0; y < summedAreaGrid.GetLength(1) - (fieldSize - 1); y++)
                {
                    long sum = summedAreaGrid[x + (fieldSize - 1), y + (fieldSize - 1)];

                    if (x != 0 && y != 0)
                    {
                        sum += summedAreaGrid[x - 1, y - 1];
                    }

                    if (x != 0)
                    {
                        sum -= summedAreaGrid[x - 1, y + (fieldSize - 1)];
                    }

                    if (y != 0)
                    {
                        sum -= summedAreaGrid[x + (fieldSize - 1), y - 1];
                    }

                    if (sum > biggest)
                    {
                        biggest = sum;
                        biggestTopLeft = new Point(x + 1, y + 1);
                    }
                }
            }
          
            cords = biggestTopLeft;
            return biggest;
        }
    }
}
