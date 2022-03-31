using FireEmblem.Domain.Data;

namespace FireEmblem.Domain.Combat
{
    public class Weapon : IItem
    {
        public string Name => Data.Name;

        public IWeaponData Data { get; }

        private Weapon(IWeaponData weaponData)
        {
            Data = weaponData;
        }

        public static Weapon Create(IWeaponData weaponData)
        {
            return new Weapon(weaponData);
        }
    }
}