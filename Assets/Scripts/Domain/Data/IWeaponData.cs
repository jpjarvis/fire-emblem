namespace FireEmblem.Domain.Data
{
    public interface IWeaponData
    {
        string Name { get; }
        int Might { get; }
        int Hit { get; }
        int Crit { get; }
        int Weight { get; }
        int MinRange { get; }
        int MaxRange { get; }
        bool IsMagic { get; }
    }
}