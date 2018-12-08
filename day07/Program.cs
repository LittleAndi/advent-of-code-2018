using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day07
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt")
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => new Step() { StepId = l.Substring(5, 1)[0], NextId = l.Substring(36, 1)[0] })
            .ToList();

            foreach (var item in lines)
            {
                System.Console.WriteLine(item);
            }
            System.Console.WriteLine();

            //Part1(lines);

            Part2(lines);
        }

        static void Part1(List<Step> lines)
        {
            List<Step> stepsDone = new List<Step>();

            List<Step> currentSteps = lines.Where(l1 => lines.Count(l2 => l2.NextId == l1.StepId) == 0).ToList();
            currentSteps.Sort();
            List<Step> endSteps = lines.Where(l1 => lines.Count(l2 => l2.StepId == l1.NextId) == 0)
            .Select(l => new Step() { StepId = l.NextId, NextId = ' ' })
            .ToList();
            lines.AddRange(endSteps);

            HashSet<char> stepOrder = new HashSet<char>();

            using (TextWriter tw = new StreamWriter("output.txt"))
            {
                while (currentSteps.Count > 0)
                {
                    var step = currentSteps.First();
                    tw.WriteLine(step);

                    stepOrder.Add(step.StepId);
                    stepsDone.AddRange(lines.Where(l => l.StepId.Equals(step.StepId)));
                    lines.RemoveAll(l => l.StepId.Equals(step.StepId));

                    currentSteps.RemoveAll(s => s.StepId.Equals(step.StepId));

                    foreach (var item in stepsDone)
                    {
                        currentSteps.AddRange(
                            lines.Where(
                                l => l.StepId.Equals(item.NextId)
                                && !currentSteps.Contains(l)
                                && currentSteps.Count(cs => cs.NextId.Equals(l.StepId)) == 0
                                && lines.Count(cs => cs.NextId.Equals(l.StepId)) == 0
                                )
                            );
                    }
                    currentSteps.Sort();
                }
            }

            foreach (var item in stepOrder)
            {
                System.Console.Write(item);
            }
            System.Console.WriteLine();

            foreach (var item in stepsDone)
            {
                System.Console.WriteLine(item);
            }

            // OCAPBFGJNQMRKSTUEIVWDXHYZL (wrong)
            // PGNRMSKTJQUEBFIVWDXHYZA (wrong)
            // PGKNRMSTJQUBEFIVWDXHYZA (didn't test but it should be wrong)
            // OCABFJKNPGQMRSTUEIVWDXHYZL (wrong)
            // PGKNOCABFJQMRSTUEIVWDXHYZL (wrong)
            // PGKLNOCABFQRSTJUEIWDXHVYMZ (wrong)
            // OCABFNPGQRSTJUEIVLWDKXHYMZ (wrong)
            // OCPUEFIXHRGWDZABTQJYMNKVSL (yay!)
        }

        static void Part2(List<Step> lines)
        {
            List<Step> stepsDone = new List<Step>();
            List<Worker> workers = new List<Worker>();

            List<Step> currentSteps = lines.Where(l1 => lines.Count(l2 => l2.NextId == l1.StepId) == 0).ToList();
            currentSteps.Sort();
            List<Step> endSteps = lines.Where(l1 => lines.Count(l2 => l2.StepId == l1.NextId) == 0)
            .Select(l => new Step() { StepId = l.NextId, NextId = ' ' })
            .ToList();
            lines.AddRange(endSteps);

            HashSet<char> stepOrder = new HashSet<char>();
            int currentSecond = 0;

            while (currentSteps.Count > 0)
            {
                while (workers.Count < 5)
                {
                    var step = currentSteps.Where(s => workers.Count(w => w.WorkingOn.StepId.Equals(s.StepId)) == 0).FirstOrDefault();
                    if (step == null) break;

                    workers.Add(new Worker { WorkingOn = step, TimeLeft = step.WorkTime } );
                }

                Console.Write($"{currentSecond}:\t");

                foreach (var worker in workers)
                {
                    Console.Write($"{worker.WorkingOn.StepId}\t");

                    worker.TimeLeft--;
                    if (worker.TimeLeft == 0) {
                        stepOrder.Add(worker.WorkingOn.StepId);
                        stepsDone.AddRange(lines.Where(l => l.StepId.Equals(worker.WorkingOn.StepId)));
                        lines.RemoveAll(l => l.StepId.Equals(worker.WorkingOn.StepId));
                        currentSteps.RemoveAll(s => s.StepId.Equals(worker.WorkingOn.StepId));

                    }
                }

                workers.RemoveAll(w => w.TimeLeft == 0);

                Console.WriteLine();

                foreach (var item in stepsDone)
                {
                    currentSteps.AddRange(
                        lines.Where(
                            l => l.StepId.Equals(item.NextId)
                            && !currentSteps.Contains(l)
                            && currentSteps.Count(cs => cs.NextId.Equals(l.StepId)) == 0
                            && lines.Count(cs => cs.NextId.Equals(l.StepId)) == 0
                            )
                        );
                }

                

                currentSteps.Sort();
                currentSecond++;
            }

            Console.WriteLine(currentSecond);

            foreach (var item in stepOrder)
            {
                System.Console.Write(item);
            }
            System.Console.WriteLine();

            foreach (var item in stepsDone)
            {
                System.Console.WriteLine(item);
            }

            // OPCUXEHFIRWZGDABTQYJMNKVSL - 991 s
        }
    }

    public class Worker
    {
        public int TimeLeft { get; set; }
        public Step WorkingOn { get; set; }
    }

    public class Step : IComparable, IEquatable<Step>
    {
        public char StepId { get; set; }
        public char NextId { get; set; }
        public string Name
        {
            get
            {
                return $"{StepId}=>{NextId}";
            }
        }
        public int WorkTime
        {
            get
            {
                return 60 + StepId - 64;
            }
        }

        public override string ToString()
        {
            return Name;
        }
        public int CompareTo(object obj)
        {
            var s = (Step)obj;
            if (s.StepId > this.StepId) return -1;
            if (s.StepId < this.StepId) return 1;
            return 0;
        }

        public bool Equals(Step other)
        {
            return other.StepId == this.StepId;
        }
    }
}
