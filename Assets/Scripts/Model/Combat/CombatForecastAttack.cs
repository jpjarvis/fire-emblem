using System;
using FireEmblem.Common;

namespace FireEmblem.Model.Combat
{
    public class CombatForecastAttack
    {
        private CombatForecastAttack(Unit attacker, Unit target, int damage, int hitChance, int critChance)
        {
            Attacker = attacker;
            Target = target;
            Damage = damage;
            HitChance = hitChance;
            CritChance = critChance;
        }
        public Unit Attacker { get; }
        public Unit Target { get; }
        public int Damage { get; }
        public int HitChance { get; }
        public int CritChance { get; }
        
        public static CombatForecastAttack Create(Unit attacker, Unit target)
        {
            var defendingStat = attacker.Weapon.Data.IsMagic ? target.Protection : target.Resilience;

            return new CombatForecastAttack(
                attacker,
                target,
                Math.Max(attacker.Attack - defendingStat, 0),
                Utils.Clamp(attacker.Hit - target.Avoid, 0, 100),
                Utils.Clamp(attacker.Crit - target.CritAvoid, 0, 100)
            );
        }
    }
}