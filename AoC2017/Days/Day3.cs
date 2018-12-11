using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text.RegularExpressions;

namespace RPSonline.AoC.E2017.Days
{

    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day3 : IDay
    {
        private const int AOC_DAY = 3;

        private readonly int _input;

        public Day3()
        {
            string input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            Regex regex = new Regex(@"\d+", RegexOptions.Compiled);
            _input = int.Parse(regex.Match(input).Value);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            int x = 0;
            int y = 0;

            int[,] gridB = new int[200, 200];
            int offsetB = 100;
            gridB[offsetB, offsetB] = 1;
            bool partB = false;

            int steps = 0;
            int stepsDirection = 1;
            int timesSteps = 0;
            Direction direction = Direction.Right;

            for (int i = 2; i <= _input; i++)
            {
                switch (direction)
                {
                    case Direction.Right:
                        x++;
                        break;
                    case Direction.Up:
                        y--;
                        break;
                    case Direction.Left:
                        x--;
                        break;
                    case Direction.Down:
                        y++;
                        break;
                }

                if(!partB)
                {
                    gridB[x + offsetB, y + offsetB] = GetSumOfNeighbour(x, y, offsetB, gridB);
                    if (gridB[x + offsetB, y + offsetB] > _input)
                    {
                        answer.PartB = gridB[x + offsetB, y + offsetB];
                        partB = true;
                    }
                }

                steps++;

                if (stepsDirection == steps)
                {
                    direction = (Direction)(((int)direction + 1) % 4);
                    timesSteps++;
                    steps = 0;
                    if (timesSteps == 2)
                    {
                        stepsDirection++;
                        timesSteps = 0;
                    }
                }
            }

            answer.PartA = Math.Abs(x) + Math.Abs(y);

            return answer;
        }

        private int GetSumOfNeighbour(int x, int y, int offset, int[,] grid)
        {
            int sum = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    sum += grid[x + offset + i, y + offset + j];
                }
            }
            return sum;
        }

        enum Direction
        {
            Right = 0,
            Up = 1,
            Left = 2,
            Down = 3
        }

    }
}
