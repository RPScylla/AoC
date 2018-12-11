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
    class Day2 : IDay
    {
        private const int AOC_DAY = 2;

        private readonly IEnumerable<string> _input;
        private readonly Regex _regex;

        public Day2()
        {
            _input = File.ReadLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex(@"\d+", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            int sum = 0;
            int sumEven = 0;

            foreach (string line in _input)
            {
                int min = int.MaxValue;
                int max = int.MinValue;
                List<int> passedNumbers = new List<int>();
                bool foundEvenDeviders = false;

                foreach (Match match in _regex.Matches(line))
                {
                    int current = int.Parse(match.Groups[0].Value);
                    min = Math.Min(current, min);
                    max = Math.Max(current, max);

                    if (!foundEvenDeviders)
                    {
                        foreach (int passed in passedNumbers)
                        {
                            if (Math.Max(current, passed) % Math.Min(current, passed) == 0)
                            {
                                sumEven += Math.Max(current, passed) / Math.Min(current, passed);
                                foundEvenDeviders = true;
                                break;
                            }
                        }
                        passedNumbers.Add(current);
                    }
                }
                sum += max - min;
            }

            answer.PartA = sum;
            answer.PartB = sumEven;

            return answer;
        }
    }
}
