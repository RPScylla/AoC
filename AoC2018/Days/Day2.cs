using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSonline.AoC.E2018.Days
{
    class Day2 : IDay
    {
        private readonly string[] _input;

        public Day2()
        {
            _input = File.ReadAllLines("Inputs/Day2.txt");
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            // Keep track of times sum is exactly 2 or 3.
            int sumExact2 = 0;
            int sumExact3 = 0;

            // True if it should do the comparison of part B.
            bool doCompare = true;

            // Keeps track of characters in string and the count of them.
            Dictionary<char, int> visit = new Dictionary<char, int>();

            // List of all old stings to compare against.
            Dictionary<string, int> compare = new Dictionary<string, int>();

            // Keep track of current input.
            int i = 0;
            foreach (string line in _input)
            {
                // If partB isnt solved yet.
                if (doCompare)
                {
                    // Get all inputs that have been checked, including current.
                    compare = _input.Take(i + 1).ToDictionary(s => s, s => 0);
                }

                // Keep track of current character.
                int j = 0;
                // Itterate over every character.
                foreach (char c in line)
                {
                    // If the character isnt fount this string add it to the dictonary.
                    if (!visit.ContainsKey(c))
                    {
                        visit.Add(c, 1);
                    }
                    else
                    {
                        // If it is found alreay increment the dictionary value.
                        visit[c]++;
                    }

                    // Part b loop over every string exept the current.
                    for (int k = compare.Count - 1; k >= 0; k--)
                    {
                        // Get the string to compare against.
                        string ckey = compare.ElementAt(k).Key;

                        // If the current character position is not equal
                        if (!ckey[j].Equals(c))
                        {
                            // Add one to the not equal value of the dictionary.
                            compare[ckey]++;
                            // if it was the second wrong character: remove the sting.
                            if (compare[ckey] >= 2)
                            {
                                compare.Remove(ckey);
                            }
                        }
                    }
                    j++;
                }

                // If the current string contains a character exactly twice increment.
                if (visit.ContainsValue(2))
                {
                    sumExact2++;
                }

                // If the current string contains a character exactly trice increment.
                if (visit.ContainsValue(3))
                {
                    sumExact3++;
                }

                // If the part B comparison yielded two results it is the answer. 
                if (compare.Count == 2)
                {
                    // Stop comparing.
                    doCompare = false;
                    // Get the two stings.
                    string a = compare.First().Key;
                    string b = compare.Last().Key;

                    StringBuilder c = new StringBuilder();
                    compare.Clear();

                    // Append all characters that match (skip the different one).
                    for (int l = 0; l < a.Length; l++)
                    {
                        if (a[l] == b[l])
                        {
                            c.Append(a[l]);
                        }
                    }

                    answer.PartB = $"{c} [{a}; {b}]";
                }

                visit.Clear();
                compare.Clear();
                i++;
            }

            answer.PartA = (sumExact2 * sumExact3);

            return answer;
        }
    }
}
