using FireEmblem.Domain.Data;

namespace FireEmblem.Tests
{
    public class TestStatBlock : IStatBlock
    {
        public TestStatBlock(int hp, int strength, int magic, int dexterity, int luck, int speed, int defence, int resistance, int movement)
        {
            Hp = hp;
            Strength = strength;
            Magic = magic;
            Dexterity = dexterity;
            Luck = luck;
            Speed = speed;
            Defence = defence;
            Resistance = resistance;
            Movement = movement;
        }
        
        public int Hp { get; }
        public int Strength { get; }
        public int Magic { get; }
        public int Dexterity { get; }
        public int Luck { get; }
        public int Speed { get; }
        public int Defence { get; }
        public int Resistance { get; }
        public int Movement { get; }
    }
}