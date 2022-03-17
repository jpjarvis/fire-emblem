using FireEmblem.Domain.Data;

namespace FireEmblem.Tests
{
    public class TestWeaponData : IWeaponData
    {
        public TestWeaponData(string name, int might, int hit, int crit, int weight, int minRange, int maxRange, bool isMagic)
        {
            Name = name;
            Might = might;
            Hit = hit;
            Crit = crit;
            Weight = weight;
            MinRange = minRange;
            MaxRange = maxRange;
            IsMagic = isMagic;
        }

        public string Name { get; }
        public int Might { get; }
        public int Hit { get; }
        public int Crit { get; }
        public int Weight { get; }
        public int MinRange { get; }
        public int MaxRange { get; }
        public bool IsMagic { get; }
    }
}