using System.Collections.Generic;

namespace FireEmblem.Domain.Data
{
    public interface IUnitData
    {
        string Name { get; }
        IStatBlock Stats { get; }
        IEnumerable<IWeaponData> Inventory { get; }
    }
}