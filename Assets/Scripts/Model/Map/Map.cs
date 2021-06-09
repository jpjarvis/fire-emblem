using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Combat;
using FireEmblem.Model.Data;
using FireEmblem.Model.Data.Serialization;

namespace FireEmblem.Model.Map
{
    public class Map
    {
        public List<MapUnit> PlayerUnits { get; }
        public List<MapUnit> EnemyUnits { get; }

        public static Map Create(MapData mapData)
        {
            var playerUnits = PlayerUnitLoader.LoadPlayerUnits();
            var map = new Map();
            foreach (var playerUnitPosition in mapData.PlayerUnitPositions)
            {
                var unit = Unit.Create(playerUnits.First());
                map.PlayerUnits.Add(new MapUnit(playerUnitPosition, unit));
            }

            foreach (var enemyUnit in mapData.EnemyUnits)
            {
                var unitData = mapData.UnitData[enemyUnit.UnitDataId];
                var unit = Unit.Create(unitData);
                map.EnemyUnits.Add(new MapUnit(enemyUnit.Position, unit));
            }

            return map;
        }
    }
}