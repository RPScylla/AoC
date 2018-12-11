using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPSonline.AoC.E2017.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day5 : IDay
    {
        private const int AOC_DAY = 5;

        private readonly string[] _input;

        public Day5()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            int[] partA = new int[_input.Length];
            int[] partB = new int[_input.Length];

            for(int i = 0; i < _input.Length; i++)
            {
                int value = int.Parse(_input[i]);
                partA[i] = value;
                partB[i] = value;
            }

            int pos = 0;
            uint cycle = 0;
            while (pos < partA.Length && pos >= 0)
            {
                int current = partA[pos];
                partA[pos] = current + 1;
                pos += current;
                cycle++;
            }


            answer.PartA = cycle;

            pos = 0;
            cycle = 0;
            while (pos < partB.Length && pos >= 0)
            {
                int current = partB[pos];
                if (current >= 3)
                {
                    partB[pos] = current - 1;
                }
                else
                {
                    partB[pos] = current + 1;
                }
                pos += current;
                cycle++;
            }

            answer.PartB = cycle;

            return answer;
        }
    }
}
