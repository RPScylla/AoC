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
    class Day12 : IDay
    {
        private const int AOC_DAY = 12;

        private readonly IEnumerable<string> _input;
        private readonly Regex _regex;

        public Day12()
        {
            _input = File.ReadLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex(@"([\.#])([\.#])([\.#])([\.#])([\.#]) => ([\.#])", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            string initline = _input.First().Remove(0, 15);
            Dictionary<long, bool> state = new Dictionary<long, bool>();

            for (int i = 0; i < initline.Length; i++)
            {
                state.Add(i, initline[i].Equals('#'));
            }

            List<Mutation> mutations = new List<Mutation>();

            foreach (string line in _input.Skip(2))
            {
                mutations.Add(new Mutation(_regex.Match(line)));
            }

            long diffPartB = 0;
            uint loopto = 100;
            for (uint i = 0; i < loopto; i++)
            {
                state = NextGeneration(state, mutations);
                if (i == 19)
                {
                    answer.PartA = state.Sum(s => s.Key);
                }
                if (i == loopto - 2)
                {
                    diffPartB = state.Sum(s => s.Key);
                }
            }

            long sum = state.Sum(s => s.Key);
            diffPartB = sum - diffPartB;

            sum += (50000000000 - loopto) * diffPartB;

            answer.PartB = sum;
            return answer;
        }

        struct Mutation
        {
            public bool[] Pattern { get; set; }
            public bool Result { get; set; }


            public Mutation(Match match)
            {
                Pattern = new bool[5];
                for (int i = 0; i < 5; i++)
                {
                    Pattern[i] = match.Groups[i + 1].Value.Equals("#");
                }
                Result = match.Groups[6].Value.Equals("#");
            }

            public bool IsMatch(bool[] pattern)
            {
                return Pattern.SequenceEqual(pattern);
            }
        }

        Dictionary<long, bool> NextGeneration(Dictionary<long, bool> state, List<Mutation> mutations)
        {
            long min = state.Min(s => s.Key) - 2;
            long max = state.Max(s => s.Key) + 2;

            Dictionary<long, bool> newState = new Dictionary<long, bool>();

            for (long i = min; i <= max; i++)
            {
                bool[] pattern = GetPattern(i, state);
                bool result = mutations.First(m => m.IsMatch(pattern)).Result;

                if (result)
                {
                    newState.Add(i, result);
                }
            }

            return newState;
        }

        bool[] GetPattern(long index, Dictionary<long, bool> state)
        {
            bool[] pattern = new bool[5];
            int j = 0;
            for (long i = index - 2; i <= index + 2; i++)
            {
                pattern[j] = state.ContainsKey(i) ? state[i] : false;
                j++;
            }
            return pattern;
        }
    }
}
