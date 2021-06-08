using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;
using FireEmblem.Model.Data.Serialization;

namespace FireEmblem.Model.Combat
{
    public class Unit
    {
        private int _damageTaken;
        public int MaxHp => Stats.Hp;
        public int CurrentHp => Math.Max(MaxHp - _damageTaken, 0);

        public List<IItem> Inventory { get; } = new List<IItem>();
        
        private readonly UnitData _unitData;

        private Unit(UnitData unitData)
        {
            Weapon = Weapon.None;
            _unitData = unitData;
        }

        public static Unit Create(UnitData unitData)
        {
            var unit = new Unit(unitData);

            foreach (var itemData in unitData.Inventory)
            {
                var weaponData = WeaponLoader.LoadWeaponData(itemData.WeaponDataId);
                unit.Inventory.Add(Weapon.Create(weaponData));
            }

            if (unit.Inventory.Any() && unit.Inventory[0] is Weapon weapon)
            {
                unit.Weapon = weapon;
            }
            
            return unit;
        }

        public string Name => _unitData.Name;
        public IStatBlock Stats => _unitData.Stats;
        public Weapon Weapon { get; set; }

        public int GetAttack()
        {
            var attackingStat = Weapon.Data.IsMagic ? Stats.Magic : Stats.Strength;
            return attackingStat + Weapon.Data.Might;
        }

        public int GetProtection()
        {
            return Stats.Defence;
        }

        public int GetResilience()
        {
            return Stats.Resistance;
        }

        public int GetAttackSpeed()
        {
            var weightPenalty = Weapon.Data.Weight - (Stats.Strength / 5);
            return Stats.Speed - Math.Max(weightPenalty, 0);
        }

        public int GetAvoid()
        {
            return GetAttackSpeed();
        }

        public int GetCrit()
        {
            return Weapon.Data.Crit + Stats.Dexterity + Stats.Luck / 2;
        }

        public int GetHit()
        {
            if (Weapon.Data.IsMagic)
            {
                return Weapon.Data.Hit + Stats.Dexterity + Stats.Luck / 2;
            }

            return Weapon.Data.Hit + Stats.Dexterity;
        }
        
        public int GetCritAvoid()
        {
            return Stats.Luck;
        }

        public bool CanFollowUp(Unit other)
        {
            return GetAttackSpeed() >= other.GetAttackSpeed() + CombatConstants.DoubleAttackSpeedDifference;
        }

        public void TakeDamage(int damage)
        {
            _damageTaken += damage;
        }

        public bool IsDead()
        {
            return CurrentHp == 0;
        }
    }
}