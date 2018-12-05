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
                            guard.ActivityList.Add(timestamp, Activity.EndShift);
                            guards.Add(guard);
                        }

                        var id = int.Parse(activity.Split(' ')[1].Remove(0, 1));
                        guard = new Guard { ID = id };
                        guard.ActivityList.Add(timestamp, Activity.BeginShift);

                        break;
                    case "falls":
                        guard.ActivityList.Add(timestamp, Activity.FallAsleep);
                        break;
                    case "wakes":
                        guard.ActivityList.Add(timestamp, Activity.WakeUp);
                        break;
                }
            }

            System.Console.WriteLine(guards.Count);

            // All guards loaded

            
        }
    }

    public class Guard
    {
        public Guard()
        {
            ActivityList = new Dictionary<DateTime, Activity>();
        }

        public int ID { get; set; }
        public Dictionary<DateTime, Activity> ActivityList { get; set; }
    }

    public enum Activity
    {
        BeginShift,
        FallAsleep,
        WakeUp,
        EndShift
    }
}
