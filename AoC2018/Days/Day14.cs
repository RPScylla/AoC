using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day14 : IDay
    {
        private const int AOC_DAY = 14;

        private readonly int _input;
        private readonly string _inputstr;

        public Day14()
        {
            _inputstr = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _input = int.Parse(_inputstr);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            List<int> ElvesIndex = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                ElvesIndex.Add(i);
            }

            StringBuilder recepies = new StringBuilder("37", 30000000);
            int saveLocation = 0;
            while (answer.PartB is null || recepies.Length <_input + 10)
            {
                int sum = 0;
                for (int j = 0; j < ElvesIndex.Count; j++)
                {
                    int current = recepies[ElvesIndex[j]] - 48;
                    sum += current;
                }

                recepies.Append(sum);

                for (int j = 0; j < ElvesIndex.Count; j++)
                {
                    int current = recepies[ElvesIndex[j]] - 48;
                    ElvesIndex[j] = (ElvesIndex[j] + current + 1) % recepies.Length;
                }

                while (answer.PartB is null && recepies.Length > saveLocation + _inputstr.Length)
                {
                    bool found = true;

                    for (int i = 0; i < _inputstr.Length; i++)
                    {
                        if(recepies[i + saveLocation] != _inputstr[i])
                        {
                            found = false;
                            break;
                        }
                    }

                    if(found)
                    {
                        answer.PartB = saveLocation;
                    }

                    saveLocation++;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                answer.PartA += recepies[i+_input].ToString();
            }

            return answer;
        }

    }
}
