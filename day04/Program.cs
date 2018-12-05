using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace day04
{
    class Program
    {
        static void Main(string[] args)
        {
            var records = File.ReadAllLines("input.txt").OrderBy(s => s);
            var guards = new List<Guard>();

            var guard = new Guard();

            foreach (var record in records)
            {
                var timestamp = DateTime.ParseExact(record.Substring(1, 16), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                var activity = record.Substring(19, record.Length - 19);

                var command = activity.Split(' ')[0];

                switch (command)
                {
                    case "Guard":
                        if (guard.ID > 0)
                        {
                            //guard.ActivityList.Add(timestamp, Activity.EndShift);
                        }

                        var id = int.Parse(activity.Split(' ')[1].Remove(0, 1));
                        guard = guards.FirstOrDefault(g => g.ID == id);
                        if (guard == null)
                        {
                            guard = new Guard { ID = id };
                            guards.Add(guard);
                        }
                        //guard.ActivityList.Add(timestamp, Activity.BeginShift);
    
                        break;
                    case "falls":
                        guard.ActivityList.Add(timestamp, Activity.FallAsleep);
                        break;
                    case "wakes":
                        guard.ActivityList.Add(timestamp, Activity.WakeUp);
                        break;
                }
            }

            // All guards loaded
            System.Console.WriteLine(guards.Count);

            // Part 1
            var worstGuard1 = guards.OrderByDescending(g => g.SleepMinutes).First();
            System.Console.WriteLine($"{worstGuard1.ID}: sleeps during {worstGuard1.SleepMinutes} minutes, worst time of the hour is {worstGuard1.MostRegularSleepMinute}. Solution {worstGuard1.ID*worstGuard1.MostRegularSleepMinute}");

            // 94542

            // Part 2
            var worstGuard2 = guards.OrderByDescending(g => g.MostRegularSleepMinuteCount).First();
            System.Console.WriteLine($"{worstGuard2.ID}: worst time of the hour is {worstGuard2.MostRegularSleepMinute} at {worstGuard2.MostRegularSleepMinuteCount} times. Solution {worstGuard2.ID*worstGuard2.MostRegularSleepMinute}");

            // 50966
        }
    }

    public class Guard : IEquatable<Guard>
    {
        public Guard()
        {
            ActivityList = new Dictionary<DateTime, Activity>();
        }

        public int ID { get; set; }
        public Dictionary<DateTime, Activity> ActivityList { get; set; }

        public int SleepMinutes
        {
            get
            {
                DateTime sleep = DateTime.MinValue;
                int sleepMinutes = 0;
                foreach (var activity in ActivityList)
                {
                    if (activity.Value == Activity.FallAsleep) sleep = activity.Key;
                    if (activity.Value == Activity.WakeUp)
                    {
                        sleepMinutes += (int)activity.Key.Subtract(sleep).TotalMinutes;
                    }
                }

                return sleepMinutes;
            }
        }

        public int MostRegularSleepMinute
        {
            get
            {
                DateTime sleep = DateTime.MinValue;
                Dictionary<int, int> sleepMinutes = new Dictionary<int, int>();
                foreach (var activity in ActivityList)
                {
                    if (activity.Value == Activity.FallAsleep) sleep = activity.Key;
                    if (activity.Value == Activity.WakeUp)
                    {
                        var wakeup = activity.Key;
                        DateTime currentDateTime = sleep;
                        while (currentDateTime < wakeup)
                        {
                            if (sleepMinutes.ContainsKey(currentDateTime.Minute))
                            {
                                sleepMinutes[currentDateTime.Minute]++;
                            }
                            else
                            {
                                sleepMinutes.Add(currentDateTime.Minute, 1);
                            }
                            currentDateTime = currentDateTime.AddMinutes(1);
                        }
                    }
                }
                return sleepMinutes.OrderByDescending(s => s.Value).First().Key;
            }
        }

        public int MostRegularSleepMinuteCount
        {
            get
            {
                if (ActivityList.Count == 0) return 0;

                DateTime sleep = DateTime.MinValue;
                Dictionary<int, int> sleepMinutes = new Dictionary<int, int>();
                foreach (var activity in ActivityList)
                {
                    if (activity.Value == Activity.FallAsleep) sleep = activity.Key;
                    if (activity.Value == Activity.WakeUp)
                    {
                        var wakeup = activity.Key;
                        DateTime currentDateTime = sleep;
                        while (currentDateTime < wakeup)
                        {
                            if (sleepMinutes.ContainsKey(currentDateTime.Minute))
                            {
                                sleepMinutes[currentDateTime.Minute]++;
                            }
                            else
                            {
                                sleepMinutes.Add(currentDateTime.Minute, 1);
                            }
                            currentDateTime = currentDateTime.AddMinutes(1);
                        }
                    }
                }
                return sleepMinutes.OrderByDescending(s => s.Value).First().Value;
            }
        }

        public bool Equals(Guard other)
        {
            return this.ID == other.ID;
        }
    }

    public enum Activity
    {
        BeginShift,
        FallAsleep,
        WakeUp,
        EndShift
    }
}
