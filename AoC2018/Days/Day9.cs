using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day9 : IDay
    {
        private const int AOC_DAY = 9;

        private readonly uint _players;
        private readonly uint _lastMarble;

        public Day9()
        {
            string input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            Regex regex = new Regex(@"(\d+) players; last marble is worth (\d+) points", RegexOptions.Compiled);
            Match match = regex.Match(input);
            _players = uint.Parse(match.Groups[1].Value);
            _lastMarble = uint.Parse(match.Groups[2].Value);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            LinkedList<uint> circle = new LinkedList<uint>();
            LinkedListNode<uint> current = circle.AddFirst(0);
            LinkedListNode<uint> remove;
            Dictionary<uint, uint> playerScore = new Dictionary<uint, uint>();

            for (uint i = 1; i <= _lastMarble * 100; i++)
            {
                if (i % 23 == 0)
                {
                    uint currentPlayer = ((i - 1) % _players) + 1;
                    AddScore(currentPlayer, i, playerScore);

                    remove = CounterClockwise(current, 7);
                    AddScore(currentPlayer, remove.Value, playerScore);

                    current = Clockwise(remove, 1);
                    circle.Remove(remove);
                }
                else
                {
                    current = Clockwise(current, 1);
                    current = circle.AddAfter(current, i);
                }

                if (i == _lastMarble)
                {
                    answer.PartA = playerScore.Max(score => score.Value);
                }
            }
            answer.PartB = playerScore.Max(score => score.Value);

            return answer;
        }

        LinkedListNode<uint> Clockwise(LinkedListNode<uint> current,uint n)
        {
            for (int i = 0; i < n; i++)
            {
                current = current.Next is null ? current.List.First : current.Next;
            }
            return current;
        }

        LinkedListNode<uint> CounterClockwise(LinkedListNode<uint> current,uint n)
        {
            for (int i = 0; i < n; i++)
            {
                current = current.Previous is null ? current.List.Last : current.Previous;
            }
            return current;
        }


        void AddScore(uint player, uint score, Dictionary<uint, uint> playerScore)
        {
            if (!playerScore.ContainsKey(player))
            {
                playerScore.Add(player, 0);
            }

            playerScore[player] += score;
        }
    }
}
