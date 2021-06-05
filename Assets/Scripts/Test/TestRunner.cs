using FireEmblem.Model.Combat;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.Test
{
    public class TestRunner : MonoBehaviour
    {
        private void PrintUnit(Unit unit)
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
        }
        
        private void Start()
        {
            var unitData = new PlayerUnitData
            {
                Name = "Edelgard",
                BaseStats = new StatBlock
                {
                    Strength = 13,
                    Magic = 5,
                    Dexterity = 7,
                    Speed = 9,
                    Defence = 7,
                    Resistance = 5
                },
                Growths = new StatBlock
                {
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
                IsMagic = false,
                MaxRange = 1,
                MinRange = 1
            };

            var weapon = Weapon.Create(weaponData);
            unit.Weapon = weapon;
            PrintUnit(unit);
        }
    }
}