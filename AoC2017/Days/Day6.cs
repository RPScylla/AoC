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
    class Day6 : IDay
    {
        private const int AOC_DAY = 6;

        private readonly string _input;
        private readonly Regex _regex;

        public Day6()
        {
            _input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex(@"\d+", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            Dictionary<int, int> memory = new Dictionary<int, int>();
            Dictionary<string, int> history = new Dictionary<string, int>();

            int i = 0;
            foreach(Match match in _regex.Matches(_input))
            {
                memory.Add(i++, int.Parse(match.Value));
            }

            int banks = i;
            bool duplicate = false;

            while(!duplicate)
            {
                history.Add(MemoryToString(memory), history.Count());

                int highest = memory.Max(m => m.Value);
                i = memory.First(m => m.Value == highest).Key;

                memory[i] = 0;

                for(int r = highest; r > 0; r--)
                {
                    i = (i + 1) % banks;
                    memory[i]++;
                }

                string currentMem = MemoryToString(memory);
                duplicate = history.ContainsKey(currentMem);
            }

            answer.PartA = history.Count();
            answer.PartB = history.Count() - history[MemoryToString(memory)];
            return answer;
        }

        private string MemoryToString(Dictionary<int, int> memory)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < memory.Count(); i++)
            {
                sb.Append(memory[i]+",");
            }
            return sb.ToString();
        }
    }
}
