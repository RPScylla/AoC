using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace RPSonline.AoC.E2018.Days
{

    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day1 : IDay
    {
        private const int AOC_DAY = 1;

        private readonly string[] _input;

        public Day1()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            int sumA = 0;
            int sumB = 0;
            foreach(string input in _input)
            {
                int fuel = int.Parse(input);
                fuel = (int)Math.Floor(fuel / 3.0f) - 2;
                sumA += fuel;
                sumB += fuel;

                while(fuel > 0)
                {
                    fuel = (int)Math.Floor(fuel / 3.0f) - 2;
                    if (fuel > 0)
                    { 
                        sumB += fuel;
                    }
                }
            }

            answer.PartA = sumA;
            answer.PartB = sumB;

            // Return the answer.
            return answer;
        }
    }
}
