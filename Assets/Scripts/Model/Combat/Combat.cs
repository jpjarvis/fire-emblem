using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Common;

namespace FireEmblem.Model.Combat
{
    public class Combat
    {
        public class CombatForecastAttack
        {
            public Unit Attacker { get; set; }
            public Unit Target { get; set; }
            public int Damage { get; set; }
            public int HitChance { get; set; }
            public int CritChance { get; set; }
        }

        private Unit _attacker;
        private Unit _defender;

        private int _distance;

        public List<CombatForecastAttack> CombatForecast { get; }
        
        public Combat()
        {
            CombatForecast = new List<CombatForecastAttack>();
        }

        public class AttackResult
        {
            public Unit Attacker { get; set; }
            public Unit Target { get; set; }
            public int Damage { get; set; }
            public bool IsHit { get; set; }
            public bool IsCrit { get; set; }
            public bool IsFatal { get; set; }
        }

        public IEnumerable<AttackResult> Resolve()
        {
            var attacks = new List<AttackResult>();
            var attackQueue = new Queue<CombatForecastAttack>(CombatForecast);

            while (attackQueue.Count > 0 && !attacks.Any(x => x.IsFatal))
            {
                var attack = attackQueue.Dequeue();
                var attackResult = new AttackResult
                {
                    Attacker = attack.Attacker,
                    Target = attack.Target,
                    IsHit = Rng.Instance.PerformCheck(attack.HitChance),
                    IsCrit = Rng.Instance.PerformCheck(attack.CritChance),
                    Damage = attack.Damage
                };
                
                if (attackResult.IsCrit)
                {
                    attackResult.Damage *= 3;
                }

                if (attackResult.IsHit)
                {
                    attackResult.Target.TakeDamage(attackResult.Damage);
                    if (attackResult.Target.IsDead())
                    {
                        attackResult.IsFatal = true;
                    }
                }
                
                attacks.Add(attackResult);
            }
            
            return attacks;
        }

        private static CombatForecastAttack GenerateCombatForecastAttack(Unit attacker, Unit target)
        {
            var defendingStat = attacker.Weapon.Data.IsMagic ? target.GetProtection() : target.GetResilience();
            
            return new CombatForecastAttack
            {
                Attacker = attacker,
                Target = target,
                Damage = Math.Max(attacker.GetAttack() - defendingStat, 0),
                HitChance = Utils.Clamp(attacker.GetHit() - target.GetAvoid(), 0, 100),
                CritChance = Utils.Clamp(attacker.GetCrit() - target.GetCritAvoid(), 0, 100),
            };
        }
        
        private void GenerateCombatForecast()
        {
            CombatForecast.Clear();
            
            // TODO Apply pre-combat skills
            
            var attackerCanAttack = _attacker.Weapon.Data.IsInRange(_distance);
            var defenderCanAttack = _defender.Weapon.Data.IsInRange(_distance);

            if (attackerCanAttack)
            {
                CombatForecast.Add(GenerateCombatForecastAttack(_attacker, _defender));
            }
            
            if (defenderCanAttack)
            {
                CombatForecast.Add(GenerateCombatForecastAttack(_defender, _attacker));
            }

            if (attackerCanAttack && _attacker.CanFollowUp(_defender))
            {
                CombatForecast.Add(GenerateCombatForecastAttack(_attacker, _defender));
            }

            if (defenderCanAttack && _defender.CanFollowUp(_attacker))
            {
                CombatForecast.Add(GenerateCombatForecastAttack(_defender, _attacker));
            }
            
            // TODO Apply attack order skills
        }
        
        public static Combat CreateCombat(Unit attacker, Unit defender, int distance)
        {
            var combat = new Combat {_attacker = attacker, _defender = defender, _distance = distance};
            combat.GenerateCombatForecast();
            return combat;
        }
    }
}