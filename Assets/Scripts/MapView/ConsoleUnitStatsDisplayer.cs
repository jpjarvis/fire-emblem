using FireEmblem.Model.Combat;
using UnityEngine;

namespace FireEmblem.MapView
{
    class ConsoleUnitStatsDisplayer : IUnitStatsDisplayer
    {
        public void DisplayStats(Unit unit)
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
    }
}