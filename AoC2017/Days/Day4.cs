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
    class Day4 : IDay
    {
        private const int AOC_DAY = 4;

        private readonly IEnumerable<string> _input;
        private readonly Regex _regex;

        public Day4()
        {
            _input = File.ReadLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex("[a-z]+", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            HashSet<string> words = new HashSet<string>();
            List<Dictionary<char, int>> wordsLetters = new List<Dictionary<char, int>>();

            int validA = 0;
            int validB = 0;

            foreach (string line in _input)
            {
                bool valid = true;
                words.Clear();
                wordsLetters.Clear();

                MatchCollection matches = _regex.Matches(line);
                foreach (Match match in matches)
                {
                    Dictionary<char, int> letters = new Dictionary<char, int>();

                    if (!words.Add(match.Value))
                    {
                        valid = false;
                    }

                    foreach (char c in match.Value)
                    {
                        if (!letters.ContainsKey(c))
                        {
                            letters.Add(c, 1);
                        }
                        else
                        {
                            letters[c]++;
                        }
                    }
                    wordsLetters.Add(letters);

                }

                if (!Anagram(wordsLetters))
                {
                    validB++;
                }

                if (valid)
                {
                    validA++;
                }
            }

            answer.PartA = validA;
            answer.PartB = validB;

            return answer;
        }

        private bool Anagram(List<Dictionary<char, int>> wordsLetters)
        {
            for (int i = 0; i < wordsLetters.Count; i++)
            {
                for (int j = (i + 1); j < wordsLetters.Count; j++)
                {
                    bool posibleAnagram = true;
                    foreach (KeyValuePair<char, int> charPair in wordsLetters[j])
                    {
                        if (!wordsLetters[i].ContainsKey(charPair.Key) ||
                            wordsLetters[i][charPair.Key] != charPair.Value)
                        {
                            posibleAnagram = false;
                            break;
                        }
                    }
                    if (posibleAnagram)
                    {
                        if (wordsLetters[i].Count == wordsLetters[j].Count)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
