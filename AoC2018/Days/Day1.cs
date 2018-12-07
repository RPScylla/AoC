using System.Collections.Generic;
using System.IO;

namespace RPSonline.AoC.E2018.Days
{
    class Day1 : IDay
    {
        private readonly string[] _input;

        public Day1()
        {
            _input = File.ReadAllLines("Inputs/Day1.txt");
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            // Keep track of current frequency.
            int frequency = 0;
            
            // Keep track of visited frequencies.
            HashSet<int> frequencyVisited = new HashSet<int>();

            // True if frequency is passed twice.
            bool revisit = false;

            // False when full input is parsed for first time.
            bool firstRun = true;

            // Loop until frequency is passed twice.
            while (!revisit)
            {
                // Itterate over every input line.
                foreach (string line in _input)
                {
                    // Parse input to integer.
                    int value = int.Parse(line);

                    // Add input to current frequency.
                    frequency += value;

                    // Only check for revisit if not yet found.
                    if (!revisit)
                    {
                        // If we cant add the frequency then a revisit occurs.
                        revisit = !frequencyVisited.Add(frequency);
                        if (revisit)
                        {
                            // Save the answer for part B.
                            answer.PartB = $"{frequency}";
                        }
                    }
                }

                // If the first run is completed save the answer for part A.
                if (firstRun)
                {
                    answer.PartA = frequency;
                    firstRun = false;
                }
            }

            // Return the answer.
            return answer;
        }
    }
}
