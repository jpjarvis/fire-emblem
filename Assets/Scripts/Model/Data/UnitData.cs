using System.Collections.Generic;
using UnityEngine;

namespace FireEmblem.Model.Data
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "Fire Emblem/Unit", order = 1)]
    public class UnitData : ScriptableObject
    {
        [SerializeField] private string name;
        [SerializeField] private StatBlock stats;
        [SerializeField] private List<WeaponData> inventory;
        
        public string Name => name;
        public StatBlock Stats => stats;

        public List<WeaponData> Inventory => inventory;
    }
}