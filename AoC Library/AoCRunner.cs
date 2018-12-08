using System;
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
        private readonly string _daysNamespace;
        private readonly string _year;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="daysNamespace">Namespace to lookup the AoC day. format: namespace.DayX where X is the daynumber</param>
        /// <param name="year">Year for in the intro message.</param>
        public AoCRunner(string daysNamespace, string year)
        {
            _daysNamespace = daysNamespace;
            _year = year;
        }

        /// <summary>
        /// First asks for day and then runs that day.
        /// </summary>
        public void Run()
        {
            // Save calling assembly to lookup days and instanciate days.
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            WriteIntroMessage(callingAssembly);

            // Read day number.
            string dayNumber = Console.ReadLine();

            // Get instance of day class.
            IDay day = callingAssembly.CreateInstance($"{_daysNamespace}.Day{dayNumber}") as IDay;

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

        private void WriteIntroMessage(Assembly callingAssembly)
        {
            // Header.
            Console.WriteLine($"AoC Solver {_year} © RPSonline.nl");

            // Day number : __ [x, x, x].
            Console.Write("Day number:    [");

            // Get all available days.
            foreach (string day in callingAssembly.GetTypes()
                                    .Where(type => type.Namespace.Equals(_daysNamespace) &&
                                                   type.Name.StartsWith("Day"))
                                    .Select(type => type.Name))
            {
                // Remove Day prefix leaving only the day number.
                Console.Write($"{day.Remove(0, 3)}, ");
            }
            // Overwrite last ','.
            Console.CursorLeft -= 2;
            Console.Write("]");

            // Put the cursor after the : [at the __].
            Console.CursorLeft = 12;
        }
    }
}
