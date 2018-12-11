using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace RPSonline.AoC.E2017.Days
{

    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day1 : IDay
    {
        private const int AOC_DAY = 1;

        private readonly string _input;

        public Day1()
        {
            _input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            int sumA = 0;
            int sumB = 0;

            int lenght = _input.Length;

            for (int i = 0; i < lenght; i++)
            {
                int nextIndex = (i + 1) % lenght;
                if (_input[i].Equals(_input[nextIndex]))
                {
                    sumA += int.Parse(_input[i].ToString());
                }

                int halfwayIndex = (i + lenght / 2) % lenght;
                if (_input[i].Equals(_input[halfwayIndex]))
                {
                    sumB += int.Parse(_input[i].ToString());
                }
            }

            answer.PartA = sumA;
            answer.PartB = sumB;

            return answer;
        }
    }
}
