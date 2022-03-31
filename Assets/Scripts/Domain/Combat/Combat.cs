using System.Collections.Generic;
using System.Linq;

namespace FireEmblem.Domain.Combat
{
    public class Combat
    {
        private IEnumerable<(bool, CombatForecastAttack)> GetAttacksInOrder(CombatForecast combatForecast)
        {
            var round = 0;

            while (round < combatForecast.Attacker.NumberOfAttacks || round < combatForecast.Defender.NumberOfAttacks)
            {
                if (round < combatForecast.Attacker.NumberOfAttacks)
                {
                    yield return (true, combatForecast.Attacker.Attack);
                }

                if (round < combatForecast.Defender.NumberOfAttacks)
                {
                    yield return (false, combatForecast.Defender.Attack);
                }

                round++;
            }
        }
        
        public CombatResult ResolveCombat(CombatForecast combatForecast)
        {
            var attacks = new List<AttackResult>();
            var forecastedAttacks = GetAttacksInOrder(combatForecast);
            using var enumerator = forecastedAttacks.GetEnumerator();

            var attacker = combatForecast.Attacker.Unit;
            var defender = combatForecast.Defender.Unit;
            
            while (!attacks.Any(x => x.IsFatal) && enumerator.MoveNext())
            {
                var (attackerIsAttacker, attack) = enumerator.Current;

                var isHit = Rng.Instance.PerformCheck(attack.HitChance);
                var isCrit = Rng.Instance.PerformCheck(attack.CritChance);
                var damage = attack.Damage;
                var isFatal = false;
                
                if (isCrit)
                {
                    damage *= CombatConstants.CritDamageMultiplier;
                }

                if (isHit)
                {
                    if (attackerIsAttacker)
                    {
                        defender = defender.TakeDamage(damage);
                        if (defender.IsDead())
                        {
                            isFatal = true;
                        }
                    }
                    else
                    {
                        attacker = attacker.TakeDamage(damage);
                        if (attacker.IsDead())
                        {
                            isFatal = true;
                        }
                    }
                }
                
                attacks.Add(new AttackResult(
                    attacker: attack.Attacker,
                    target: attack.Target,
                    isHit: isHit,
                    isCrit: isCrit, 
                    damage: damage,
                    isFatal: isFatal
                ));
            }
            
            return new CombatResult(attacker, defender, attacks);
        }
    }
}