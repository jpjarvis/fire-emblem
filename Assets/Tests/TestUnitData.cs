using System.Collections.Generic;
using FireEmblem.Domain.Data;

namespace FireEmblem.Tests
{
    public class TestUnitData : IUnitData
    {
        public TestUnitData(string name, IStatBlock stats, IEnumerable<IWeaponData> inventory)
        {
            Name = name;
            Stats = stats;
            Inventory = inventory;
        }

        public string Name { get; }
        public IStatBlock Stats { get; }
        public IEnumerable<IWeaponData> Inventory { get; }
    }
}