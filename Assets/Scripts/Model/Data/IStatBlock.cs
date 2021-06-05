namespace FireEmblem.Model.Data
{
    public interface IStatBlock
    {
        int Hp { get; }
        int Strength { get; }
        int Magic { get; }
        int Dexterity { get; }
        int Luck { get; }
        int Speed { get; }
        int Defence { get; }
        int Resistance { get; }
    }
}