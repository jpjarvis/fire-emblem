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

        public static readonly Weapon None = new Weapon(
            new WeaponData
            {
                Name = "None",
                Might = 0,
                Crit = 0,
                Hit = 100,
                IsMagic = false,
                MaxRange = 1,
                MinRange = 1
            });

        public static Weapon Create(WeaponData weaponData)
        {
            return new Weapon(weaponData);
        }
    }
}