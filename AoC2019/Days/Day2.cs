using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace RPSonline.AoC.E2018.Days
{

    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day2 : IDay
    {
        private const int AOC_DAY = 2;

        private readonly string _input;
        private readonly int _partBGoal;
        private int[] _computer;

        public Day2()
        {
            _input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _partBGoal = 19690720;
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            ResetComputer();

            _computer[1] = 12;
            _computer[2] = 2;

            RunComputer();

            answer.PartA = _computer[0];


            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    ResetComputer();
                    _computer[1] = noun;
                    _computer[2] = verb;
                    RunComputer();

                    if (_computer[0] == _partBGoal)
                    {
                        answer.PartB = 100 * noun + verb;
                        break;
                    }
                }

                if(answer.PartB != null)
                {
                    break;
                }
            }


            // Return the answer.
            return answer;
        }

        private void RunComputer()
        {
            int computerPosition = 0;
            bool halted = false;
            while (!halted)
            {

                switch (_computer[computerPosition])
                {
                    case 1:
                        Add(computerPosition);
                        computerPosition += 4;
                        break;
                    case 2:
                        Multiply(computerPosition);
                        computerPosition += 4;
                        break;
                    case 99:
                        halted = true;
                        break;
                    default:
                        computerPosition++;
                        break;
                }
            }
        }

        private void Multiply(int computerPosition)
        {
            int indexA = _computer[computerPosition + 1];
            int indexB = _computer[computerPosition + 2];
            int indexC = _computer[computerPosition + 3];
            _computer[indexC] = _computer[indexA] * _computer[indexB];
        }

        private void Add(int computerPosition)
        {
            int indexA = _computer[computerPosition + 1];
            int indexB = _computer[computerPosition + 2];
            int indexC = _computer[computerPosition + 3];
            _computer[indexC] = _computer[indexA] + _computer[indexB];
        }

        private void ResetComputer()
        {
            string[] parts = _input.Split(',');
            _computer = parts.Select(p => int.Parse(p)).ToArray();
        }
    }
}
