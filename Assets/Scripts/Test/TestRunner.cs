using FireEmblem.Model.Combat;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.Test
{
    public class TestRunner : MonoBehaviour
    {
        private static void PrintUnit(Unit unit)
        {
            Debug.Log($"{unit.Name} wielding {unit.Weapon.Name}");
            Debug.Log("STR: " + unit.Stats.Strength);
            Debug.Log("MAG: " + unit.Stats.Magic);
            Debug.Log("DEX: " + unit.Stats.Dexterity);
            Debug.Log("LCK: " + unit.Stats.Luck);
            Debug.Log("SPE: " + unit.Stats.Speed);
            Debug.Log("DEF: " + unit.Stats.Defence);
            Debug.Log("RES: " + unit.Stats.Resistance);
            
            Debug.Log($"Attack: {unit.GetAttack()}");
            Debug.Log($"Hit: {unit.GetHit()}");
            Debug.Log($"Crit: {unit.GetCrit()}");
            Debug.Log($"AS: {unit.GetAttackSpeed()}");
        }
        
        private void Start()
        {
            var playerUnit = CreatePlayerUnit();
            var enemyUnit = CreateBandit();

            var combat = Combat.Create(playerUnit, enemyUnit, 2);
            foreach (var attack in combat.CombatForecast)
            {
                Debug.Log(
                    $"{attack.Attacker.Name} will attack {attack.Target.Name} with {attack.Attacker.Weapon.Name}\n" +
                    $"Damage: {attack.Damage}\n" +
                    $"Hit chance: {attack.HitChance}\n" +
                    $"Crit chance: {attack.CritChance}");
            }

            var combatAttacks = combat.Resolve();
            foreach (var attack in combatAttacks)
            {
                Debug.Log(
                    $"{attack.Attacker.Name} attacks {attack.Target.Name} with {attack.Attacker.Weapon.Name}"
                );
                if (attack.IsCrit)
                {
                    Debug.Log("CRIT!");
                }
                if (attack.IsHit)
                {
                    Debug.Log($"{attack.Target.Name} takes {attack.Damage} damage!");
                }
                else
                {
                    Debug.Log("But it missed!");
                }

                if (attack.IsFatal)
                {
                    Debug.Log($"{attack.Target.Name} was slain by {attack.Attacker.Name}!");
                }
            }
        }

        private static PlayerUnit CreatePlayerUnit()
        {
            var unitData = new PlayerUnitData
            {
                Name = "Edelgard",
                BaseStats = new StatBlock
                {
                    Hp = 28,
                    Strength = 13,
                    Magic = 5,
                    Dexterity = 7,
                    Speed = 9,
                    Defence = 7,
                    Resistance = 5
                },
                Growths = new StatBlock
                {
                    Hp = 50,
                    Strength = 50,
                    Magic = 30,
                    Dexterity = 45,
                    Speed = 50,
                    Defence = 40,
                    Resistance = 30
                }
            };

            var unit = new PlayerUnit(unitData);

            var weaponData = new WeaponData
            {
                Name = "Aymr",
                Might = 18,
                Crit = 20,
                Hit = 60,
                Weight = 10,
                IsMagic = false,
                MaxRange = 1,
                MinRange = 1
            };

            var weapon = Weapon.Create(weaponData);
            unit.Weapon = weapon;
            return unit;
        }
        
        private static Unit CreateBandit()
        {
            var unitData = new PlayerUnitData
            {
                Name = "Bandit",
                BaseStats = new StatBlock
                {
                    Hp = 54,
                    Strength = 7,
                    Magic = 1,
                    Dexterity = 5,
                    Speed = 20,
                    Defence = 5,
                    Resistance = 2
                },
                Growths = new StatBlock
                {
                    Hp = 0,
                    Strength = 50,
                    Magic = 30,
                    Dexterity = 45,
                    Speed = 50,
                    Defence = 40,
                    Resistance = 30
                }
            };

            var unit = new PlayerUnit(unitData);

            var weaponData = new WeaponData
            {
                Name = "Fire",
                Might = 5,
                Crit = 0,
                Hit = 80,
                Weight = 5,
                IsMagic = true,
                MaxRange = 3,
                MinRange = 2
            };

            var weapon = Weapon.Create(weaponData);
            unit.Weapon = weapon;
            return unit;
        }
    }
}