using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;

namespace FireEmblem.Model.Combat
{
    public class Unit
    {
        private readonly UnitData _unitData;
        public int MaxHp => Stats.Hp;
        public int Hp { get; }

        public IEnumerable<IItem> Inventory { get; }
        public string Name => _unitData.Name;
        public IStatBlock Stats => _unitData.Stats;
        public Weapon Weapon { get; }
        
        public int Attack => Weapon.Data.IsMagic ? Stats.Magic : Stats.Strength + Weapon.Data.Might;

        public int Protection => Stats.Defence;

        public int Resilience => Stats.Resistance;

        public int AttackSpeed => Stats.Speed - Math.Max(Weapon.Data.Weight - (Stats.Strength / 5), 0);

        public int Avoid => AttackSpeed;

        public int Crit => Weapon.Data.Crit + Stats.Dexterity + Stats.Luck / 2;

        public int Hit 
        {
            get
            {
                if (Weapon.Data.IsMagic)
                {
                    return Weapon.Data.Hit + Stats.Dexterity + Stats.Luck / 2;
                }

                return Weapon.Data.Hit + Stats.Dexterity;
            }
        }
        
        public int CritAvoid => Stats.Luck;
        
        private Unit(UnitData unitData, IEnumerable<IItem> inventory, int hp)
        {
            _unitData = unitData;
            Inventory = inventory;
            Weapon = Inventory.FirstOrDefault(x => x is Weapon) as Weapon;
            Hp = hp;
        }

        public static Unit Create(UnitData unitData)
        {
            return new Unit(unitData, unitData.Inventory.Select(Weapon.Create), unitData.Stats.Hp);
        }

        public bool CanFollowUp(Unit other)
        {
            return AttackSpeed >= other.AttackSpeed + CombatConstants.DoubleAttackSpeedDifference;
        }

        public Unit TakeDamage(int damage)
        {
            return new Unit(_unitData, Inventory, Math.Max(Hp - damage, 0));
        }

        public bool IsDead()
        {
            return Hp == 0;
        }
    }
}