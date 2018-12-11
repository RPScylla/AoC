using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text.RegularExpressions;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day3 : IDay
    {
        private const int AOC_DAY = 3;

        private readonly string[] _input;
        private readonly Regex _regex;

        public Day3()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            // Regex "#digits @ digits,digits: digitsxdigits".
            _regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();
            List<Claim> claims = new List<Claim>();

            // Keep track of biggest sizes in both directions.
            int sizeX = 0;
            int sizeY = 0;

            // Read every line in input.
            foreach (string line in _input)
            {
                // Get claim out of line.
                Match match = _regex.Match(line);
                Claim claim = new Claim(match);
                claims.Add(claim);

                // Check for biggest size.
                if (claim.X + claim.Width > sizeX)
                {
                    sizeX = claim.X + claim.Width;
                }

                if (claim.Y + claim.Height > sizeY)
                {
                    sizeY = claim.Y + claim.Height;
                }
            }

            // Create grid based on biggest size.
            int[,] grid = new int[sizeX, sizeY];

            // Coutn overlapping squares.
            int overlappingSquares = 0;
            // Keep track of claims that when they were created didnt overlap.
            List<Claim> posibleNoneOverlap = new List<Claim>();

            // Itterate over claims.
            foreach (Claim claim in claims)
            {
                // keep track if claim overlaps.
                bool overlap = false;

                // Itterate over every square in claim.
                for (int x = claim.X; x < claim.X + claim.Width; x++)
                {
                    for (int y = claim.Y; y < claim.Y + claim.Height; y++)
                    {
                        // If the grid was already visited this claim overlaps.
                        if (grid[x, y] != 0)
                        {
                            overlap = true;
                        }

                        // If the grid is visited for the second time this is the first time the square overlaps.
                        if (grid[x, y] == 1)
                        {
                            overlappingSquares++;
                        }

                        // Increment the square.
                        grid[x, y]++;
                    }
                }

                // If this claim didnt overlap keep track of it for later.
                if (!overlap)
                {
                    posibleNoneOverlap.Add(claim);
                }
            }


            // Recheck all initialy not overlapping claims if they stil not overlap.
            foreach (Claim claim in posibleNoneOverlap)
            {
                if (CheckIfNotOverlapping(claim, grid))
                {
                    answer.PartB = claim.Id;
                    break;
                }
            }

            answer.PartA = overlappingSquares;

            return answer;
        }

        private bool CheckIfNotOverlapping(Claim claim, int[,] grid)
        {
            // Scan the parts of the grid of this claim if they are all 1 (only this claim)
            for (int x = claim.X; x < claim.X + claim.Width; x++)
            {
                for (int y = claim.Y; y < claim.Y + claim.Height; y++)
                {
                    if (grid[x, y] != 1)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        struct Claim
        {
            public int Id;
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public Claim(Match match)
            {
                Id = int.Parse(match.Groups[1].Value);
                X = int.Parse(match.Groups[2].Value);
                Y = int.Parse(match.Groups[3].Value);
                Width = int.Parse(match.Groups[4].Value);
                Height = int.Parse(match.Groups[5].Value);
            }
        }
    }
}
