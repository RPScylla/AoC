using System;
using System.IO;
using System.Text;

namespace RPSonline.AoC.E2018.Days
{
    class Day5 : IDay
    {
        private readonly string _input;

        public Day5()
        {
            _input = File.ReadAllText("Inputs/Day5.txt");
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            string shortend = RemoveReactingTypes(_input);
            answer.PartA = shortend.Length;

            int shortest = _input.Length;
            char shortestChar = ' ';

            for (int c = 65; c <= 90; c++)
            {
                string removedChars = _input.Replace(((char)c).ToString(), "").Replace(((char)(c+32)).ToString(), "");
                shortend = RemoveReactingTypes(removedChars);
                if(shortend.Length < shortest)
                {
                    shortest = shortend.Length;
                    shortestChar = (char)c;
                }
            }

            answer.PartB = $"{shortest} (char: {shortestChar})";

            return answer;
        }

        private string RemoveReactingTypes(string value)
        {
            bool removed = true;
            StringBuilder removeFrom = new StringBuilder(value);
            while (removed)
            {
                removed = false;
                char nextReaction = ' ';

                for (int i = removeFrom.Length - 1; i >= 0; i--)
                {
                    if (removeFrom[i].Equals(nextReaction))
                    {
                        removed = true;
                        removeFrom = removeFrom.Remove(i, 2);
                    }

                    i = Math.Min(removeFrom.Length - 1, i);
                    nextReaction = removeFrom[i];
                    if (nextReaction >= 97)
                    {
                        nextReaction = (char)(nextReaction - 32);
                    }
                    else
                    {
                        nextReaction = (char)(nextReaction + 32);
                    }
                }
            }
            return removeFrom.ToString();
        }
    }
}
