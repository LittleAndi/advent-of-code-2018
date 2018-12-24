using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var groups = ReadGroups("input.txt");

            //Fight((List<Group>)groups.Clone());

            int boost = 0;
            var testGroups = (List<Group>)groups.Clone();
            Console.WriteLine($"Boost {boost}:");
            while (Fight(testGroups, boost) != ArmyType.ImmuneSystem)
            {
                boost++;
                testGroups = (List<Group>)groups.Clone(); // new copy
                Console.WriteLine($"Boost {boost}:");
            }

            // 1074 (too high!)

            Console.WriteLine(boost);
        }

        private static ArmyType Fight(List<Group> groups, int boost = 0)
        {
            bool done = false;
            int round = 0;
            ArmyType result = ArmyType.Infection;

            groups.ForEach((item) => { if (item.Army == ArmyType.ImmuneSystem) item.Boost = boost; });

            while (!done)
            {
                //Console.WriteLine($"Round {++round}:");

                // Target Selection Phase
                Dictionary<Group, Group> attackChoices = new Dictionary<Group, Group>();
                foreach (var attackingGroup in groups.Where(g => g.Units > 0).OrderByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative))
                {
                    //Console.WriteLine($"Selecting targets for {attackingGroup}");

                    var enemyToAttack = groups.Where(g => g.Units > 0 && g.Army != attackingGroup.Army && !attackChoices.ContainsValue(g))
                        .OrderByDescending(g => g.DamageByAttacker(attackingGroup))
                        .ThenByDescending(g => g.EffectivePower)
                        .FirstOrDefault();

                    if (enemyToAttack == null) continue;

                    attackChoices.Add(attackingGroup, enemyToAttack);

                    //Console.WriteLine($"\tSelected {enemyToAttack}");
                }

                // Attack Phase
                foreach (var attackRound in attackChoices.OrderByDescending(g => g.Key.Initiative))
                {
                    //Console.WriteLine($"{attackRound.Key} attacks {attackRound.Value}");
                    if (attackRound.Key.Units > 0)
                    {
                        attackRound.Value.AttackedBy(attackRound.Key);
                    }
                }

                if (groups.Where(g => g.Army.Equals(ArmyType.ImmuneSystem) && g.Units > 0).Count() == 0)
                {
                    done = true;
                    Console.WriteLine($"Infection wins with {groups.Where(g => g.Army.Equals(ArmyType.Infection)).Sum(g => g.Units)} units left");
                    result = ArmyType.Infection;
                }
                if (groups.Where(g => g.Army.Equals(ArmyType.Infection) && g.Units > 0).Count() == 0)
                {
                    done = true;
                    Console.WriteLine($"Immune System wins with {groups.Where(g => g.Army.Equals(ArmyType.ImmuneSystem)).Sum(g => g.Units)} units left");
                    result = ArmyType.ImmuneSystem;
                }
            }
            // 33649 (too high!)
            // 33112 (too low!)
            // 33551 (yay!)

            return result;
        }

        private static List<Group> ReadGroups(string file)
        {
            List<Group> groups = new List<Group>();

            var lines = File.ReadAllLines(file)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            Regex regex = new Regex(@"(\d+) units each with (\d+) hit points (\(((\w+) to (\w+[,\s]*)+)[;\s]*((\w+) to (\w+[,\s]*)+)?\))?\s?with an attack that does (\d+) (\w+) damage at initiative (\d+)");

            ArmyType armyType = ArmyType.ImmuneSystem;

            foreach (var line in lines)
            {
                if (line.Equals("Immune System:")) { armyType = ArmyType.ImmuneSystem; continue; }
                if (line.Equals("Infection:")) { armyType = ArmyType.Infection; continue; }

                MatchCollection matches = regex.Matches(line);

                foreach (Match match in matches)
                {
                    var matchGroups = match.Groups;
                    var units = int.Parse(matchGroups[1].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);
                    var hitPoints = int.Parse(matchGroups[2].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);
                    var attackPoints = int.Parse(matchGroups[10].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);
                    var attackType = (AttackType)Enum.Parse(typeof(AttackType), matchGroups[11].ToString());
                    var initiative = int.Parse(matchGroups[12].ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);

                    var group = new Group(armyType, units, hitPoints, new AttackType[] { }, new AttackType[] { }, attackPoints, attackType, initiative);

                    if (matchGroups[5].Success)
                    {
                        var type = matchGroups[5].ToString();
                        var s = matchGroups[4].ToString().Replace($"{type} to ", "").Split(", ").Select(t => (AttackType)Enum.Parse(typeof(AttackType), t)).ToArray();
                        if (matchGroups[5].ToString().Equals("immune")) { group.Immunes = s; }
                        if (matchGroups[5].ToString().Equals("weak")) { group.Weaknesses = s; }
                    }
                    if (matchGroups[8].Success)
                    {
                        var type = matchGroups[8].ToString();
                        var s = matchGroups[7].ToString().Replace($"{type} to ", "").Split(", ").Select(t => (AttackType)Enum.Parse(typeof(AttackType), t)).ToArray();
                        if (matchGroups[8].ToString().Equals("immune")) { group.Immunes = s; }
                        if (matchGroups[8].ToString().Equals("weak")) { group.Weaknesses = s; }
                    }
                    groups.Add(group);
                }
            }

            return groups;
        }
    }

    public class Group : IEquatable<Group>, ICloneable
    {
        private int _attackPoints;
        public Group(ArmyType army, int units, int hitPoints, AttackType[] weaknesses, AttackType[] immunes, int attackPoints, AttackType attackType, int initiative, int boost = 0)
        {
            Army = army;
            Units = units;
            HitPoints = hitPoints;
            Weaknesses = weaknesses;
            Immunes = immunes;
            AttackPoints = attackPoints;
            AttackType = attackType;
            Initiative = initiative;
            Boost = boost;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public ArmyType Army { get; set; }
        public int Units { get; set; }
        public int HitPoints { get; set; }
        public AttackType[] Weaknesses { get; set; }
        public AttackType[] Immunes { get; set; }
        public int AttackPoints { get => _attackPoints + Boost; set => _attackPoints = value; }
        public AttackType AttackType { get; set; }
        public int Initiative { get; set; }
        public int Boost { get; set; }
        public int EffectivePower => Units * AttackPoints;

        public object Clone()
        {
            return new Group(Army, Units, HitPoints, Weaknesses, Immunes, AttackPoints, AttackType, Initiative, Boost);
        }

        public bool Equals(Group other)
        {
            return other.Id == Id;
        }

        public override string ToString()
        {
            return $"{Army} with {Units} units and effective power {EffectivePower}";
        }

        internal void AttackedBy(Group attackingGroup)
        {
            var damage = DamageByAttacker(attackingGroup);
            Units -= Convert.ToInt32(damage / HitPoints);
            Units = Units < 0 ? 0 : Units;
        }

        internal int DamageByAttacker(Group attackingGroup)
        {
            if (Immunes.Contains(attackingGroup.AttackType)) return 0;
            if (Weaknesses.Contains(attackingGroup.AttackType)) return attackingGroup.EffectivePower * 2;
            return attackingGroup.EffectivePower;
        }
    }

    public enum ArmyType
    {
        ImmuneSystem,
        Infection
    }

    public enum AttackType
    {
        cold,
        fire,
        slashing,
        radiation,
        bludgeoning
    }

    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
