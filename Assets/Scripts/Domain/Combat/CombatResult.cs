using System.Collections.Generic;

namespace FireEmblem.Domain.Combat
{
    public class CombatResult
    {
        public CombatResult(Unit attacker, Unit defender, IEnumerable<AttackResult> attackResults)
        {
            Attacker = attacker;
            Defender = defender;
            AttackResults = attackResults;
        }

        public Unit Attacker { get; }
        public Unit Defender { get; }
        
        public IEnumerable<AttackResult> AttackResults { get; }

        public string ToText()
        {
            var messages = new List<string>();
            messages.Add($"{Attacker.Name} attacks {Defender.Name}!");
            foreach (var attack in AttackResults)
            {
                messages.Add($"{(attack.IsFromAttacker ? Attacker.Name : Defender.Name)} attacks...");
                
                if (attack.IsCrit)
                {
                    messages.Add("CRIT!");
                }

                if (attack.IsHit)
                {
                    messages.Add($"Dealt {attack.Damage} damage!");
                }
                else
                {
                    messages.Add("But it missed!");
                }

                if (attack.IsFatal)
                {
                    messages.Add($"{(attack.IsFromAttacker ? Defender.Name : Attacker.Name)} was slain!");
                }
                
            }

            return string.Join("\n", messages);
        }
    }
}