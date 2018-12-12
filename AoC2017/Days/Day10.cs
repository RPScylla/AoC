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
    class Day10 : IDay
    {
        private const int AOC_DAY = 10;

        private readonly string _input;

        public Day10()
        {
            _input = File.ReadAllText(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            // Part A.
            byte[] list = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

            IEnumerable<byte> partAsteps = _input.Split(',').Select(i => byte.Parse(i));
            list = SparseHash(list, partAsteps);

            answer.PartA = list[0] * list[1];

            // Part B.
            IEnumerable<byte> partBsteps = _input.Select(x => (byte)x);
            partBsteps = partBsteps.Concat(new byte[] { 0x11, 0x1F, 0x49, 0x2F, 0x17 });

            list = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            list = SparseHash(list, partBsteps, 64);
            int blocksize = 16;
            byte[] denseHash = DenseHash(list, blocksize);

            answer.PartB = string.Join("", denseHash.Select(x => x.ToString("x2")));

            return answer;
        }

        private byte[] SparseHash(byte[] list, IEnumerable<byte> steps, int repeats = 1)
        {
            int skipSize = 0;
            int currentPos = 0;

            for (int r = 0; r < repeats; r++)
            {
                foreach (byte subsetLength in steps)
                {
                    byte[] sublist = new byte[subsetLength];
                    for (int i = 0; i < subsetLength; i++)
                    {
                        sublist[i] = list[(currentPos + i) % list.Length];
                    }

                    sublist = sublist.Reverse().ToArray();

                    for (int i = 0; i < subsetLength; i++)
                    {
                        list[(currentPos + i) % list.Length] = sublist[i];
                    }

                    currentPos += (subsetLength + skipSize++) % list.Length;
                }
            }
            return list;
        }

        public byte[] DenseHash(byte[] list, int blocksize)
        {
            byte[] denseHash = new byte[list.Length / blocksize];
            for (int i = 0; i < list.Length / blocksize; i++)
            {
                byte xord = 0x00;
                foreach(byte b in list.Skip(i * blocksize).Take(blocksize))
                {
                    xord ^= b;
                }
                denseHash[i] = xord;
            }
            return denseHash;
        }
    }
}
