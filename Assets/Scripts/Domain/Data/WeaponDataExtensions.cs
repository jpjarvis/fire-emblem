namespace FireEmblem.Domain.Data
{
    public static class WeaponDataExtensions
    {
        public static bool IsInRange(this IWeaponData weaponData, int range)
        {
            return range >= weaponData.MinRange && range <= weaponData.MaxRange;
        }
    }
}