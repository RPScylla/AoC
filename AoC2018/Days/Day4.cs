
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPSonline.AoC.E2018.Days
{
    [Export(typeof(IDay))]
    [ExportMetadata("Year", Consts.AOC_YEAR)]
    [ExportMetadata("Day", AOC_DAY)]
    class Day4 : IDay
    {
        private const int AOC_DAY = 4;

        private readonly string[] _input;
        private readonly Regex _regex;

        public Day4()
        {
            _input = File.ReadAllLines(FileHelper.GetInputLocation(AOC_DAY, Consts.AOC_YEAR));
            _regex = new Regex(@"\[(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2})\] (?:Guard #(\d+) begins shift|(falls asleep)|(wakes up))", RegexOptions.Compiled);
        }

        public AnswerModel Solve()
        {
            AnswerModel answer = new AnswerModel();

            List<LogEntry> logEntries = new List<LogEntry>();
            Dictionary<int, Guard> guards = new Dictionary<int, Guard>();

            foreach (string line in _input)
            {
                // Create a log entry out of every line.
                Match match = _regex.Match(line);
                LogEntry entry = new LogEntry(match);
                logEntries.Add(entry);

                // If the logentries contains a guard that is not found earlyer add the guard.
                if (entry.GuardId != 0 && !guards.ContainsKey(entry.GuardId))
                {
                    guards.Add(entry.GuardId, new Guard(entry.GuardId));
                }
            }

            // Sort the logentries based on there timestamp.
            logEntries.Sort((x, y) => x.TimeStamp.CompareTo(y.TimeStamp));


            Guard currentGuard = null;
            // keep track of last time asleep.
            LogEntry lastAsleep = new LogEntry();

            // Itterate over logentries.
            foreach (LogEntry logEntry in logEntries)
            {
                switch (logEntry.Event)
                {
                    case LogEvent.StartShift:
                        currentGuard = guards[logEntry.GuardId];
                        break;
                    case LogEvent.FallAsleep:
                        lastAsleep = logEntry;
                        break;
                    case LogEvent.WakeUp:
                        currentGuard.AddSleep(lastAsleep, logEntry);
                        break;
                }
            }

            Guard mostSleep = guards.OrderByDescending(guard => guard.Value.TotalAsleep).First().Value;
            answer.PartA = (mostSleep.Id * mostSleep.HighestMinute);

            Guard highFrequency = guards.OrderByDescending(guard => guard.Value.HighestFrequency).First().Value;
            answer.PartB = (highFrequency.Id * highFrequency.HighestMinute);

            return answer;
        }

        class Guard
        {
            public int Id { get; set; }
            public int[] Asleep { get; private set; }
            public int TotalAsleep { get => Asleep.Sum(); }
            public int HighestMinute { get => Array.FindIndex(Asleep, item => item == Asleep.Max()); }
            public int HighestFrequency { get => Asleep.Max(); }

            public Guard(int id)
            {
                Id = id;
                Asleep = new int[60];
            }

            public void AddSleep(LogEntry asleep, LogEntry awake)
            {
                for (int i = asleep.TimeStamp.Minute; i < awake.TimeStamp.Minute; i++)
                {
                    Asleep[i]++;
                }
            }
        }

        struct LogEntry
        {
            public int GuardId { get; set; }
            public LogEvent Event { get; set; }
            public DateTime TimeStamp { get; set; }

            public LogEntry(Match match)
            {
                int year = int.Parse(match.Groups[1].Value);
                int month = int.Parse(match.Groups[2].Value);
                int day = int.Parse(match.Groups[3].Value);
                int hour = int.Parse(match.Groups[4].Value);
                int minute = int.Parse(match.Groups[5].Value);

                TimeStamp = new DateTime(year, month, day, hour, minute, 0);

                if (match.Groups[6].Success)
                {
                    GuardId = int.Parse(match.Groups[6].Value);
                    Event = LogEvent.StartShift;
                }
                else
                {
                    GuardId = 0;

                    if (match.Groups[7].Success)
                    {
                        Event = LogEvent.FallAsleep;
                    }
                    else if (match.Groups[8].Success)
                    {
                        Event = LogEvent.WakeUp;
                    }
                    else
                    {
                        Event = LogEvent.Unknown;
                    }
                }
            }
        }

        enum LogEvent
        {
            StartShift,
            FallAsleep,
            WakeUp,
            Unknown
        }
    }
}
