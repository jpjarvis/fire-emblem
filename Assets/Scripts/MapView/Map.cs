using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private TileObjectManager tileObjectManager;
        [SerializeField] private MapGrid mapGrid;
        private List<PlayerUnit> PlayerUnits { get; set; } = new List<PlayerUnit>();
        private List<EnemyUnit> EnemyUnits { get; set; } = new List<EnemyUnit>();

        private BaseUnit _selectedUnit;
        private List<AccessibleTile> _accessibleTiles;

        private void Awake()
        {
            PlayerUnits = GetComponentsInChildren<PlayerUnit>().ToList();
            EnemyUnits = GetComponentsInChildren<EnemyUnit>().ToList();
        }

        private void SelectUnit(BaseUnit unit)
        {
            _selectedUnit = unit;
            tileObjectManager.DestroyAll();

            _accessibleTiles = GenerateAccessibleTiles(unit);
            ShowAccessibleTiles(_accessibleTiles);
        }

        public void SelectCell(MapPosition position)
        {
            if (_selectedUnit != null)
            {
                var tile = _accessibleTiles.FirstOrDefault(t => t.Position.Equals(position));
                if (tile is { Accessibility: TileAccessibility.CanMoveTo })
                {
                    mapGrid.MoveObjectToGridPosition(_selectedUnit.gameObject, position);
                }

                tileObjectManager.DestroyAll();
                _selectedUnit = null;
                return;
            }

            var selectedPlayerUnit = PlayerUnits.FirstOrDefault(u => u.Position.Equals(position));
            if (selectedPlayerUnit)
            {
                SelectUnit(selectedPlayerUnit);
                DisplayStats(selectedPlayerUnit);
            }
        }

        private static void DisplayStats(BaseUnit unit)
        {
            Debug.Log($"{unit.Unit.Name} wielding {unit.Unit.Weapon.Name}");
            Debug.Log("STR: " + unit.Unit.Stats.Strength);
            Debug.Log("MAG: " + unit.Unit.Stats.Magic);
            Debug.Log("DEX: " + unit.Unit.Stats.Dexterity);
            Debug.Log("LCK: " + unit.Unit.Stats.Luck);
            Debug.Log("SPE: " + unit.Unit.Stats.Speed);
            Debug.Log("DEF: " + unit.Unit.Stats.Defence);
            Debug.Log("RES: " + unit.Unit.Stats.Resistance);

            Debug.Log($"Attack: {unit.Unit.GetAttack()}");
            Debug.Log($"Hit: {unit.Unit.GetHit()}");
            Debug.Log($"Crit: {unit.Unit.GetCrit()}");
            Debug.Log($"AS: {unit.Unit.GetAttackSpeed()}");
        }

        private void ShowAccessibleTiles(List<AccessibleTile> accessibleTiles)
        {
            foreach (var tile in accessibleTiles)
            {
                tileObjectManager.CreateMoveTile(tile.Position);
            }
        }

        private bool CanMoveThrough(MapPosition mapPosition)
        {
            return !EnemyUnits.Any(unit => unit.Position.Equals(mapPosition)) &&
                   mapGrid.GetTileAt(mapPosition).IsTraversable;
        }

        private List<AccessibleTile> GenerateAccessibleTiles(BaseUnit unit)
        {
            var startPosition = unit.Position;
            var maximumMoveDistance = unit.Unit.Stats.Movement;
            var minAttackRange = unit.Unit.Weapon.Data.MinRange;
            var maxAttackRange = unit.Unit.Weapon.Data.MaxRange;

            var workingTiles = new List<MapPosition> { startPosition };
            var accessibleTiles = new List<AccessibleTile>();

            var spacesMoved = 0;

            while (spacesMoved < maximumMoveDistance && workingTiles.Any())
            {
                var newWorkingTiles = new List<MapPosition>();
                foreach (var tile in workingTiles)
                {
                    var tilesToCheck = new List<MapPosition>
                    {
                        new MapPosition(tile.X + 1, tile.Y),
                        new MapPosition(tile.X - 1, tile.Y),
                        new MapPosition(tile.X, tile.Y + 1),
                        new MapPosition(tile.X, tile.Y - 1),
                    };

                    foreach (var tileToCheck in tilesToCheck)
                    {
                        if (CanMoveThrough(tileToCheck)
                            && !accessibleTiles.Any(t => t.Position.Equals(tileToCheck))
                            && !newWorkingTiles.Any(t => t.Equals(tileToCheck))
                            && !tileToCheck.Equals(startPosition))
                        {
                            accessibleTiles.Add(new AccessibleTile
                            {
                                Position = tileToCheck,
                                Accessibility = TileAccessibility.CanMoveTo
                            });
                            newWorkingTiles.Add(tileToCheck);
                        }
                    }
                }

                workingTiles = newWorkingTiles;
                spacesMoved++;
            }

            return accessibleTiles;
        }
    }

    public class AccessibleTile
    {
        public MapPosition Position { get; set; }
        public TileAccessibility Accessibility { get; set; }
    }

    public enum TileAccessibility
    {
        Inaccessible,
        CanMoveTo,
        CanAttack
    }
}