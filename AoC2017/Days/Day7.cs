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
    class Day7 : IDay
    {
        private const int AOC_DAY = 7;

        private readonly IEnumerable<string> _input;
        private readonly Regex _regex;

        public Day7()
        {
            _input = File.ReadLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex(@"([a-z]+)\s*\((\d+)\)\s*(?:->\s*(.*))?", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            foreach (string line in _input)
            {
                Node node = new Node(_regex.Match(line));
                nodes.Add(node.Name, node);
            }

            foreach(KeyValuePair<string, Node> node in nodes)
            {
                if(!string.IsNullOrWhiteSpace(node.Value.ChildrenString))
                {
                    string[] children = node.Value.ChildrenString.Split(',');
                    foreach(string child in children)
                    {
                        node.Value.Children.Add(nodes[child.Trim()]);
                        nodes[child.Trim()].Parent = node.Value;
                    }
                }
            }

            Node current = nodes.First().Value;
            while(current.Parent != null)
            {
                current = current.Parent;
            }

            answer.PartA = current.Name;

            current.CalculateWeight();

            bool unblance = true;
            int normalWeight = 0;

            while (unblance)
            {
                IEnumerable<int> childWeight = current.Children.Select(c => c.WeightTotal);
                if (childWeight.Count(c => c == childWeight.First()) > 1)
                {
                    normalWeight = childWeight.First();
                }
                else
                {
                    normalWeight = childWeight.Last();
                }

                current = current.Children.First(c => c.WeightTotal != normalWeight);

                int childFirstWeight = current.Children.First().WeightTotal;
                unblance = !current.Children.All(c => c.WeightTotal == childFirstWeight);
            }
            

            answer.PartB = current.Weight + (normalWeight - current.WeightTotal);

            return answer;
        }

        class Node
        {
            public string Name { get; set; }
            public int Weight { get; set; }
            public int WeightTotal { get; set; }
            public string ChildrenString { get; set; }
            public List<Node> Children { get; set; }
            public Node Parent { get; set; }

            public Node(Match match)
            {
                Children = new List<Node>();
                Name = match.Groups[1].Value;
                Weight = int.Parse(match.Groups[2].Value);
                ChildrenString = match.Groups[3].Value;
            }

            public int CalculateWeight()
            {
                int sum = Weight;
                foreach(Node child in Children)
                {
                    sum += child.CalculateWeight();
                }
                WeightTotal = sum;
                return sum;
            }
        }
    }
}
