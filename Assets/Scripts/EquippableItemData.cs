namespace FireEmblem
{
    public class EquippableItemData
    {
        // null if item is not a weapon
        public WeaponData WeaponData { get; set; }
        
        public int Weight { get; set; }

        public bool IsWeapon()
        {
            return WeaponData != null;
        }
        
        public static readonly EquippableItemData Empty = new EquippableItemData
        {
            WeaponData = WeaponData.Empty,
            Weight = 0
        };
    }
}