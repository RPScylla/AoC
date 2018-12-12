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
    class Day8 : IDay
    {
        private const int AOC_DAY = 8;

        private readonly IEnumerable<string> _input;
        private readonly Regex _regex;

        public Day8()
        {
            _input = File.ReadLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            // g[1]: register, g[2]: inc/dec, g[3]: value, g[4]: compareRegister, g[5]: comparison, g[6] compareValue
            _regex = new Regex(@"([a-z]+) (inc|dec) (-?\d+) if ([a-z]+) ([><=!]+) (-?\d+)", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            Dictionary<string, int> registers = new Dictionary<string, int>();
            int highest = int.MinValue;

            foreach (string line in _input)
            {
                Match match = _regex.Match(line);
                GroupCollection g = match.Groups;
                if(Compare(registers, g[4].Value, g[5].Value, int.Parse(g[6].Value)))
                {
                    int diff = int.Parse(g[3].Value);
                    if(g[2].Value.Equals("dec"))
                    {
                        diff *= -1;
                    }
                    int current = WriteToRegister(registers, g[1].Value, diff);

                    if(current > highest)
                    {
                        highest = current;
                    }
                }
            }

            answer.PartA = registers.Max(r => r.Value);
            answer.PartB = highest;
            return answer;
        }

        private bool Compare(Dictionary<string, int> register, string key, string operation, int value)
        {
            int regValue = ReadFromRegister(register, key);

            switch (operation)
            {
                case ">":
                    return regValue > value;
                case "<":
                    return regValue < value;
                case "<=":
                    return regValue <= value;
                case ">=":
                    return regValue >= value;
                case "!=":
                    return regValue != value;
                case "==":
                    return regValue == value;
            }
            throw new NotImplementedException();
        }

        private int WriteToRegister(Dictionary<string, int> register, string key, int difference)
        {
            if (!register.ContainsKey(key))
            {
                register.Add(key, 0);
            }

            register[key] += difference;
            return register[key];
        }

        private int ReadFromRegister(Dictionary<string, int> register, string key)
        {
            if (!register.ContainsKey(key))
            {
                return 0;
            }

            return register[key];
        }
    }
}
