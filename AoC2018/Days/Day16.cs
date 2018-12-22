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
    class Day16 : IDay
    {
        private const int AOC_DAY = 16;

        private readonly string[] _inputa;
        private readonly string[] _inputb;

        public Day16()
        {
            string[] input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            bool lastLineWhite = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (lastLineWhite && string.IsNullOrWhiteSpace(input[i]))
                {
                    _inputa = input.Take(i - 1).ToArray();
                    _inputb = input.Skip(i + 2).ToArray();
                    break;
                }
                lastLineWhite = string.IsNullOrWhiteSpace(input[i]);
            }
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            Dictionary<int, HashSet<OpCode>> posibilities = new Dictionary<int, HashSet<OpCode>>();
            Dictionary<int, HashSet<OpCode>> wrongs = new Dictionary<int, HashSet<OpCode>>();
            List<OpCode> codes = new List<OpCode>();
            int partA = 0;
            int[] registers = new int[4];

            for (int i = 0; i < 16; i++)
            {
                posibilities.Add(i, new HashSet<OpCode>());
                wrongs.Add(i, new HashSet<OpCode>());
                codes.Add((OpCode)i);
            }


            for (int block = 0; block < _inputa.Length; block += 4)
            {
                int[] beforeParts = _inputa[block].Substring(9, _inputa[block].Length - 10).Split(',').Select(s => int.Parse(s.Trim())).ToArray();
                int[] instructionParts = _inputa[block + 1].Split(' ').Select(s => int.Parse(s.Trim())).ToArray();
                int[] afterParts = _inputa[block + 2].Substring(9, _inputa[block + 2].Length - 10).Split(',').Select(s => int.Parse(s.Trim())).ToArray();

                int correct = 0;
                foreach (OpCode opCode in codes)
                {
                    beforeParts.CopyTo(registers, 0);
                    Execute(opCode, registers, instructionParts[1], instructionParts[2], instructionParts[3]);

                    if (registers.SequenceEqual(afterParts))
                    {
                        correct++;
                        if (!wrongs[instructionParts[0]].Contains(opCode))
                        {
                            posibilities[instructionParts[0]].Add(opCode);
                        }
                    }
                    if (!registers.SequenceEqual(afterParts))
                    {
                        wrongs[instructionParts[0]].Add(opCode);
                        posibilities[instructionParts[0]].Remove(opCode);
                    }
                }
                if (correct >= 3)
                {
                    partA++;
                }
            }

            answer.PartA = partA;


            Dictionary<int, OpCode> opCodes = new Dictionary<int, OpCode>();

            while (opCodes.Count < 16)
            {
                KeyValuePair<int, HashSet<OpCode>> current = posibilities.FirstOrDefault(p => p.Value.Except(opCodes.Select(o => o.Value)).Count() == 1);
                opCodes.Add(current.Key, current.Value.Except(opCodes.Select(o => o.Value)).First());
            }

            registers = new int[4] { 0, 0, 0, 0 };

            foreach(string line in _inputb)
            {
                int[] instructions = line.Split(' ').Select(s => int.Parse(s.Trim())).ToArray();
                Execute(opCodes[instructions[0]], registers, instructions[1], instructions[2], instructions[3]);
            }

            answer.PartB = registers[0];

            return answer;
        }

        private void Execute(OpCode opCode, int[] registers, int a, int b, int c)
        {
            switch (opCode)
            {
                case OpCode.addr:
                    registers[c] = registers[a] + registers[b];
                    break;
                case OpCode.addi:
                    registers[c] = registers[a] + b;
                    break;
                case OpCode.mulr:
                    registers[c] = registers[a] * registers[b];
                    break;
                case OpCode.muli:
                    registers[c] = registers[a] * b;
                    break;
                case OpCode.banr:
                    registers[c] = registers[a] & registers[b];
                    break;
                case OpCode.bani:
                    registers[c] = registers[a] & b;
                    break;
                case OpCode.borr:
                    registers[c] = registers[a] | registers[b];
                    break;
                case OpCode.bori:
                    registers[c] = registers[a] | b;
                    break;
                case OpCode.setr:
                    registers[c] = registers[a];
                    break;
                case OpCode.seti:
                    registers[c] = a;
                    break;
                case OpCode.gtir:
                    registers[c] = a > registers[b] ? 1 : 0;
                    break;
                case OpCode.gtri:
                    registers[c] = registers[a] > b ? 1 : 0;
                    break;
                case OpCode.gtrr:
                    registers[c] = registers[a] > registers[b] ? 1 : 0;
                    break;
                case OpCode.eqir:
                    registers[c] = a == registers[b] ? 1 : 0;
                    break;
                case OpCode.eqri:
                    registers[c] = registers[a] == b ? 1 : 0;
                    break;
                case OpCode.eqrr:
                    registers[c] = registers[a] == registers[b] ? 1 : 0;
                    break;

            }

        }

        enum OpCode
        {
            addr,
            addi,
            mulr,
            muli,
            banr,
            bani,
            borr,
            bori,
            setr,
            seti,
            gtir,
            gtri,
            gtrr,
            eqir,
            eqri,
            eqrr
        }
    }
}
