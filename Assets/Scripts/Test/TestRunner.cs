using System.Linq;
using FireEmblem.Model.Combat;
using FireEmblem.Model.Data.Serialization;
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
            var map = MapLoader.LoadMap("Test");
            var playerUnits = PlayerUnitLoader.LoadPlayerUnits();
            var playerUnit = Unit.Create(playerUnits.First());
            var enemyUnit = Unit.Create(map.UnitData[map.EnemyUnits.First().UnitDataId]);

            var combat = Combat.Create(playerUnit, enemyUnit, 1);
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
    }
}