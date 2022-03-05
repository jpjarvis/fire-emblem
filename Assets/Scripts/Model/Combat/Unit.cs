﻿using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;

namespace FireEmblem.Model.Combat
{
    public class Unit
    {
        private readonly UnitData _unitData;
        private int _damageTaken;
        public int MaxHp => Stats.Hp;
        public int CurrentHp => Math.Max(MaxHp - _damageTaken, 0);

        public IEnumerable<IItem> Inventory { get; private set; } = new List<IItem>();
        public string Name => _unitData.Name;
        public IStatBlock Stats => _unitData.Stats;
        public Weapon Weapon { get; private set; }
        
        private Unit(UnitData unitData)
        {
            _unitData = unitData;
        }

        public static Unit Create(UnitData unitData)
        {
            var unit = new Unit(unitData);
            
            unit.Inventory = unitData.Inventory.Select(Weapon.Create);
            unit.Weapon = unit.Inventory.FirstOrDefault(x => x is Weapon) as Weapon;
            
            return unit;
        }

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