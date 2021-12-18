using UnityEngine;

namespace FireEmblem.Model.Data
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Fire Emblem/Weapon", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private string name;
        [SerializeField] private int might;
        [SerializeField] private int hit;
        [SerializeField] private int crit;
        [SerializeField] private int weight;
        [SerializeField] private int minRange;
        [SerializeField] private int maxRange;
        [SerializeField] private bool isMagic;

        public string Name => name;

        public int Might => might;

        public int Hit => hit;

        public int Crit => crit;

        public int Weight => weight;

        public int MinRange => minRange;

        public int MaxRange => maxRange;

        public bool IsMagic => isMagic;

        public bool IsInRange(int range)
        {
            return range >= MinRange && range <= MaxRange;
        }
    }
}