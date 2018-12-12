using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RPSonline.AoC.E2017.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day9 : IDay
    {
        private const int AOC_DAY = 9;

        private readonly string _input;

        public Day9()
        {
            _input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            int sum = 0;
            int groupLevel = 0;
            bool garbage = false;
            bool ignoreNext = false;

            int sumGarbage = 0;

            foreach (char c in _input)
            {
                if (ignoreNext)
                {
                    ignoreNext = false;
                }
                else if (c.Equals('!'))
                {
                    ignoreNext = true;
                }
                else if (garbage && c.Equals('>'))
                {
                    garbage = false;
                }
                else if(!garbage)
                {
                    if(c.Equals('<'))
                    {
                        garbage = true;
                    }
                    else if(c.Equals('{'))
                    {
                        groupLevel++;
                    }
                    else if(c.Equals('}'))
                    {
                        sum += groupLevel;
                        groupLevel--;
                    }
                }
                else
                {
                    sumGarbage++;
                }
            }

            answer.PartA = sum;
            answer.PartB = sumGarbage;
            return answer;
        }
    }
}
