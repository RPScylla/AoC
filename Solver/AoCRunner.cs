using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RPSonline.AoC
{
    /// <summary>
    /// AoC runner.
    /// </summary>
    public class AoCRunner
    {

        // Field is not null, mef filled.
#pragma warning disable CS0649, IDE0044
        [ImportMany]
        private IEnumerable<Lazy<IDay, IDayData>> _days;
#pragma warning restore CS0649, IDE044



        /// <summary>
        /// First asks for day and then runs that day.
        /// </summary>
        public void Run()
        {
            // Header.
            Console.WriteLine($"AoC Solver © RPSonline.nl");

            // Get the year number
            int yearNumber = GetYear();

            if(yearNumber == -1)
            {
                Console.WriteLine("No years found!");
                return;
            }

            // Read day number.
            int dayNumber = GetDay(yearNumber);

            if (dayNumber == -1)
            {
                Console.WriteLine("No days found!");
                return;
            }

            // Get instance of day class.
            IDay day = _days.FirstOrDefault(d => d.Metadata.Year == yearNumber && d.Metadata.Day == dayNumber)?.Value;

            // If day doesn't exist write message and quit.
            if (day is null)
            {
                Console.WriteLine("Cant find solver");
            }
            // If day does exist.
            else
            {
                // Stopwatch to see execution time.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Solve day.
                AnswerModel answer = day.Solve();

                stopwatch.Stop();

                // Write answers and stopwatch time.
                Console.WriteLine($"Part A: {answer.PartA}");
                Console.WriteLine($"Part B: {answer.PartB}");
                Console.WriteLine($"[Calculated in {stopwatch.Elapsed.TotalSeconds} seconds]");
            }
        }

        private int GetYear()
        {
            HashSet<int> years = new HashSet<int>();

            foreach (Lazy<IDay, IDayData> day in _days)
            {
                years.Add(day.Metadata.Year);
            }

            if (years.Count == 0)
            {
                return -1;
            }
            else if (years.Count == 1)
            {
                return years.First();
            }

            // year number : ____ [xxxx, xxxx, xxxx].
            Console.Write("Year number:      [");

            // Get all available years.
            foreach (int year in years.OrderBy(y => y))
            {
                Console.Write($"{year}, ");
            }

            // Overwrite last ','.
            Console.CursorLeft -= 2;
            Console.Write("]");

            // Put the cursor after the : [at the ____].
            Console.CursorLeft = 13;

            string input = Console.ReadLine();
            return int.Parse(input);
        }

        private int GetDay(int year)
        {
            HashSet<int> days = new HashSet<int>();

            foreach (Lazy<IDay, IDayData> day in _days)
            {
                if (day.Metadata.Year == year)
                {
                    days.Add(day.Metadata.Day);
                }
            }

            if (days.Count == 0)
            {
                return -1;
            }
            else if (days.Count == 1)
            {
                return days.First();
            }

            // day number : __ [xx, xx, xx].
            Console.Write("Day number:    [");

            // Get all available years.
            foreach (int day in days.OrderBy(y => y))
            {
                Console.Write($"{day}, ");
            }

            // Overwrite last ','.
            Console.CursorLeft -= 2;
            Console.Write("]");

            // Put the cursor after the : [at the __].
            Console.CursorLeft = 12;

            string input = Console.ReadLine();
            return int.Parse(input);
        }
    }
}
