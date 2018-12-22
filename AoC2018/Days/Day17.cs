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
    class Day17 : IDay
    {
        private const int AOC_DAY = 17;

        private readonly string[] _input;

        public Day17()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
        }

        public AnswerModel Solve()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            AnswerModel answer = new AnswerModel();
            List<Point> clays = new List<Point>();
            foreach (string line in _input)
            {
                string[] parts = line.Split(',');
                int first = int.Parse(parts[0].Substring(2));
                int from = int.Parse(parts[1].Substring(3, parts[1].IndexOf("..") - 3));
                int to = int.Parse(parts[1].Substring(parts[1].IndexOf("..") + 2));

                for (int i = from; i <= to; i++)
                {
                    if (parts[0][0] == 'x')
                    {
                        clays.Add(new Point(first, i));
                    }
                    else
                    {
                        clays.Add(new Point(i, first));
                    }
                }
            }


            int minX = clays.Min(p => p.X) - 3;
            int maxX = clays.Max(p => p.X) + 3;
            int minY = clays.Min(p => p.Y);
            Tile[,] map = new Tile[clays.Max(p => p.Y) + 1, maxX - minX + 1];

            foreach (Point clay in clays)
            {
                map[clay.Y, clay.X - minX] = Tile.Clay;
            }
            map[0, 500 - minX] = Tile.Source;
            FlowFom(map, 0, 500 - minX);

            answer.PartA = map.Cast<Tile>().Where(t => t == Tile.Flowing || t == Tile.Water).Count() - minY + 1;
            answer.PartB = map.Cast<Tile>().Where(t => t == Tile.Water).Count();
            return answer;
        }

        private void FlowFom(Tile[,] map, int y, int x)
        {
            if (y >= map.GetLength(0) - 1) { return; }
            if (map[y + 1, x] == Tile.Sand)
            {
                map[y + 1, x] = Tile.Flowing;
                FlowFom(map, y + 1, x);
            }
            for (int dx = -1; dx <= 1; dx += 2)
            {
                if ((map[y + 1, x] == Tile.Clay || map[y + 1, x] == Tile.Water) && map[y, x + dx] == Tile.Sand)
                {
                    map[y, x + dx] = Tile.Flowing;
                    FlowFom(map, y, x + dx);
                }
            }
            if (BetweenClay(map, y, x, out int startPosX, out int endPosX))
            {
                for (int cx = startPosX + 1; cx < endPosX; cx++)
                {
                    map[y, cx] = Tile.Water;
                }
            }
        }

        private bool BetweenClay(Tile[,] map, int y, int x, out int startPosX, out int endPosX)
        {
            startPosX = x; endPosX = x;
            return CheckBetweenClay(map, y, x, -1, out startPosX) && CheckBetweenClay(map, y, x, 1, out endPosX);
        }
        private bool CheckBetweenClay(Tile[,] map, int y, int x, int direction, out int curPosX)
        {
            while (true)
            {
                curPosX = x;
                if (map[y, x] == Tile.Sand)
                {
                    return false;
                }
                else if (map[y, x] == Tile.Clay)
                {
                    return true;
                }
                x += direction;
            }
        }

        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        enum Tile
        {
            Sand,
            Source,
            Clay,
            Flowing,
            Water
        }
    }
}
