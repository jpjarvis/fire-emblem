using FireEmblem.Model.Data;

namespace FireEmblem.Model.Combat
{
    public class Weapon : IItem
    {
        public string Name => Data.Name;

        public WeaponData Data { get; }

        public Durability Durability { get; }

        private Weapon(WeaponData weaponData)
        {
            Data = weaponData;
        }

        public static Weapon Create(WeaponData weaponData)
        {
            return new Weapon(weaponData);
        }
    }
}