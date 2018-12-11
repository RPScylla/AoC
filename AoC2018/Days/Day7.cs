using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day7 : IDay
    {
        private const int AOC_DAY = 7;

        private readonly string[] _input;
        private readonly Regex _regex;

        public Day7()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex("Step ([A-Za-z]) must be finished before step ([A-Za-z]) can begin.", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            foreach (string line in _input)
            {
                Match match = _regex.Match(line);
                string current = match.Groups[1].Value;
                string next = match.Groups[2].Value;

                if (!nodes.ContainsKey(current))
                {
                    nodes.Add(current, new Node(current));
                }

                if (!nodes.ContainsKey(next))
                {
                    nodes.Add(next, new Node(next));
                }

                nodes[next].AddPreReq(current);
                nodes[current].AddPreReqOff(next);
            }

            answer.PartA = CalculateA(nodes);

            answer.PartB = CalculateB(nodes);

            return answer;
        }

        private string CalculateA(Dictionary<string, Node> nodes)
        {
            StringBuilder ans = new StringBuilder();

            while (ans.Length < nodes.Count)
            {
                ans.Append(nodes.Where(node => node.Value.Available(ans.ToString())).OrderBy(node => node.Key).First().Key);
            }

            return ans.ToString();
        }

        private int CalculateB(Dictionary<string, Node> nodes)
        {
            StringBuilder taken = new StringBuilder();
            StringBuilder processed = new StringBuilder();
            int ticks = -1;

            List<Worker> workers = new List<Worker>();
            for (int i = 0; i < 5; i++)
            {
                workers.Add(new Worker());
            }

            while (processed.Length < nodes.Count)
            {
                ticks++;
                workers.ForEach(worker => worker.Tick());

                foreach (Worker worker in workers.Where(worker => worker.Done))
                {
                    processed.Append(worker.Retrive());
                }

                foreach (Worker worker in workers.Where(worker => worker.Available))
                {
                    IEnumerable<Node> available = nodes.Where(node =>
                                        node.Value.Available(processed.ToString())
                                        && !taken.ToString().Contains(node.Key))
                                        .OrderByDescending(node => node.Value.PrereqOff.Count())
                                        .Select(node => node.Value);

                    Node choise = available.FirstOrDefault();

                    if (choise is null)
                    {
                        continue;
                    }

                    taken.Append(choise.ID);
                    worker.Start(choise);
                }
            }

            return ticks;
        }

        class Worker
        {
            private bool _done;
            private int _time;
            public string _id;

            public bool Available { get => _done && _id.Length == 0; }
            public bool Done { get => _done && _id.Length > 0; }

            public Worker()
            {
                _done = true;
                _id = "";
            }

            public void Start(Node node)
            {
                _id = node.ID;
                _time = node.ProcessTime;
                _done = false;
            }

            public void Tick()
            {
                if (!_done)
                {
                    _time--;
                }

                if (_time == 0)
                {
                    _done = true;
                }
            }

            public string Retrive()
            {
                if (!_done)
                {
                    return null;
                }

                string temp = _id;

                _id = "";
                _time = 0;

                return temp;
            }
        }

        class Node
        {
            public string ID { get; set; }
            public List<string> Prereq { get; set; }
            public List<string> PrereqOff { get; set; }
            public int ProcessTime { get => (ID[0] - 64) + 60; }

            public Node(string id)
            {
                ID = id;
                Prereq = new List<string>();
                PrereqOff = new List<string>();
            }

            public void AddPreReq(string node)
            {
                Prereq.Add(node);
            }

            public void AddPreReqOff(string node)
            {
                PrereqOff.Add(node);
            }

            public bool Available(string completed)
            {
                return !completed.Contains(ID) && Prereq.All(pre => completed.Contains(pre));
            }
        }
    }
}
