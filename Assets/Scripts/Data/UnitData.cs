using System.Collections.Generic;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.Data
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "Fire Emblem/Unit", order = 1)]
    public class UnitData : ScriptableObject, IUnitData
    {
        [SerializeField] private string name;
        [SerializeField] private StatBlock stats;
        [SerializeField] private List<WeaponData> inventory;
        
        public string Name => name;
        public IStatBlock Stats => stats;

        public IEnumerable<IWeaponData> Inventory => inventory;
    }
}