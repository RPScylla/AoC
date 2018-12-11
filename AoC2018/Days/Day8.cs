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
    class Day8 : IDay
    {
        private const int AOC_DAY = 8;

        private readonly string[] _input;

        public Day8()
        {
            _input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR)).Split(' ');
        }


        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            Node current = null;
            Stack<Node> nodesToProccess = new Stack<Node>();

            int metadataSum = 0;

            foreach (string line in _input)
            {
                int value = int.Parse(line);

                if (current is null)
                {
                    current = new Node(value);
                }
                else if (!current.SetupDone)
                {
                    current.SetMetadataCount(value);
                }
                else if (!current.AllChildren)
                {
                    Node child = new Node(value);
                    current.AddChild(child);
                    nodesToProccess.Push(current);
                    current = child;
                }
                else
                {
                    current.AddMetadata(value);
                    metadataSum += value;
                    if (current.AllMetadata && nodesToProccess.Count > 0)
                    {
                        current = nodesToProccess.Pop();
                    }
                }
            }

            answer.PartA = metadataSum;

            answer.PartB = current.GetPartBValue();

            return answer;
        }

        class Node
        {
            private int _childrenCount;
            private int _metadataCount;

            public bool SetupDone { get; private set; }
            public List<Node> Children { get; private set; }
            public List<int> Metadata { get; private set; }

            public bool AllChildren { get => _childrenCount == 0; }
            public bool AllMetadata { get => _metadataCount == 0; }

            public Node(int children)
            {
                Children = new List<Node>();
                Metadata = new List<int>();
                _childrenCount = children;
                SetupDone = false;
            }

            public void SetMetadataCount(int metadata)
            {
                _metadataCount = metadata;
                SetupDone = true;
            }

            public void AddChild(Node child)
            {
                Children.Add(child);
                _childrenCount--;
            }

            public void AddMetadata(int metadata)
            {
                Metadata.Add(metadata);
                _metadataCount--;
            }

            public int GetPartBValue()
            {
                if (Children.Count == 0)
                {
                    return Metadata.Sum();
                }

                int sum = 0;
                foreach (int index in Metadata)
                {
                    if (index - 1 < Children.Count)
                    {
                        sum += Children[index - 1].GetPartBValue();
                    }
                }
                return sum;
            }
        }
    }
}
