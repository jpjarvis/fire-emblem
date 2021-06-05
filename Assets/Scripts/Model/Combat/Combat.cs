using System;
using System.Collections.Generic;
using FireEmblem.Common;

namespace FireEmblem.Model.Combat
{
    public class Combat
    {
        private class CombatForecastAttack
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

        private readonly List<CombatForecastAttack> _combatForecast;
        
        public Combat()
        {
            _combatForecast = new List<CombatForecastAttack>();
        }

        public class AttackResult
        {
            public Unit Target { get; set; }
            public int Damage { get; set; }
            public bool IsHit { get; set; }
            public bool IsCrit { get; set; }
        }

        public IEnumerable<AttackResult> Resolve()
        {
            var attacks = new List<AttackResult>();
            var attackQueue = new Queue<CombatForecastAttack>(_combatForecast);

            while (attackQueue.Count > 0)
            {
                var attack = attackQueue.Dequeue();
                var attackResult = new AttackResult
                {
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
                    // attackResult.Target.TakeDamage(attackResult.Damage);
                }
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
            _combatForecast.Clear();
            
            // TODO Apply pre-combat skills
            
            var attackerCanAttack = _attacker.Weapon.Data.IsInRange(_distance);
            var defenderCanAttack = _defender.Weapon.Data.IsInRange(_distance);

            if (attackerCanAttack)
            {
                _combatForecast.Add(GenerateCombatForecastAttack(_attacker, _defender));
            }
            
            if (defenderCanAttack)
            {
                _combatForecast.Add(GenerateCombatForecastAttack(_defender, _attacker));
            }

            if (attackerCanAttack && _attacker.CanFollowUp(_defender))
            {
                _combatForecast.Add(GenerateCombatForecastAttack(_attacker, _defender));
            }

            if (defenderCanAttack && _defender.CanFollowUp(_attacker))
            {
                _combatForecast.Add(GenerateCombatForecastAttack(_defender, _attacker));
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