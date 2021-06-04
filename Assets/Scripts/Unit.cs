using System;

namespace FireEmblem
{
    public class Unit
    {
        public string Name { get; set; }
        
        public StatBlock BaseStats { get; }

        public EquippedWeapon Weapon { get; private set; }
        
        public EquippedItem EquippedItem { get; private set; }

        private int _currentHp;
        
        public Unit()
        {
            BaseStats = new StatBlock();
            Weapon = new EquippedWeapon();
            EquippedItem = new EquippedItem();
        }

        public int GetStat(Stat stat)
        {
            // Will include skill effects once they are added
            return BaseStats.Get(stat);
        }

        public int GetAttack()
        {
            return GetStat(Weapon.WeaponData.AttackStat) + Weapon.WeaponData.Might;
        }

        public int GetProtection()
        {
            return GetStat(Stat.Defense);
        }

        public int GetResilience()
        {
            return GetStat(Stat.Resistance);
        }

        public int GetAttackSpeed()
        {
            var weightPenalty = Weapon.EquippableItemData.Weight + EquippedItem.EquippableItemData.Weight - (GetStat(Stat.Strength) / 5);
            return GetStat(Stat.Speed) - Math.Max(weightPenalty, 0);
        }

        public int GetAvoid()
        {
            return GetAttackSpeed();
        }

        public int GetCrit()
        {
            return Weapon.WeaponData.Crit + (GetStat(Stat.Dexterity) + GetStat(Stat.Luck) / 2);
        }

        public int GetHit()
        {
            if (Weapon.WeaponData.AttackStat == Stat.Magic)
            {
                return Weapon.WeaponData.Hit + (GetStat(Stat.Dexterity) + GetStat(Stat.Luck) / 2);
            }

            return Weapon.WeaponData.Hit + GetStat(Stat.Dexterity);
        }

        public int GetCritAvoid()
        {
            return GetStat(Stat.Luck);
        }

        public bool CanFollowUp(Unit other)
        {
            return GetAttackSpeed() >= other.GetAttackSpeed() + 4;
        }
    }
}