using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;
using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class Map : MonoBehaviour
    {
        private TileObjectManager _tileObjectManager;
        private IUnitStatsDisplayer _unitStatsDisplayer;
        private List<PlayerUnit> PlayerUnits { get; set; } = new List<PlayerUnit>();
        private List<EnemyUnit> EnemyUnits { get; set; } = new List<EnemyUnit>();

        private BaseUnit _selectedUnit;
        private Grid _grid;

        [Inject]
        public void Init(TileObjectManager tileObjectManager, IUnitStatsDisplayer unitStatsDisplayer, Grid grid)
        {
            PlayerUnits = GetComponentsInChildren<PlayerUnit>().ToList();
            EnemyUnits = GetComponentsInChildren<EnemyUnit>().ToList();
            
            _tileObjectManager = tileObjectManager;
            _unitStatsDisplayer = unitStatsDisplayer;
            _grid = grid;
        }
        
        private void SelectUnit(BaseUnit mapUnit)
        {
            _selectedUnit = mapUnit;
            _tileObjectManager.DestroyAll();
            ShowMovementRange(mapUnit);
        }

        public void SelectCell(MapPosition position)
        {
            var selectedPlayerUnit = PlayerUnits.FirstOrDefault(u => u.Position.Equals(position));
            if (selectedPlayerUnit)
            {
                SelectUnit(selectedPlayerUnit);
                _unitStatsDisplayer.DisplayStats(selectedPlayerUnit.Unit);
            }
        }

        private void MoveObjectToGridPosition(Component component, MapPosition position)
        {
            component.transform.localPosition = _grid.GetCellCenterLocal(new Vector3Int(position.X, position.Y, 0));
        }
        
        private void ShowMovementRange(BaseUnit mapUnit)
        {
            var tiles = GenerateAccessibleTiles(mapUnit);

            foreach (var tile in tiles)
            {
                _tileObjectManager.CreateMoveTile(tile.Position.X, tile.Position.Y, () =>
                {
                    if (_selectedUnit != null)
                    {
                        MoveObjectToGridPosition(_selectedUnit, tile.Position);
                    }
                    _tileObjectManager.DestroyAll();
                });
            }
        }

        private bool CanMoveThrough(MapPosition mapPosition)
        {
            return !EnemyUnits.Any(unit => unit.Position.Equals(mapPosition));
        }

        private List<AccessibleTile> GenerateAccessibleTiles(BaseUnit unit)
        {
            var startPosition = unit.Position;
            var maximumMoveDistance = unit.Unit.Stats.Movement;
            var minAttackRange = unit.Unit.Weapon.Data.MinRange;
            var maxAttackRange = unit.Unit.Weapon.Data.MaxRange;

            var workingTiles = new List<MapPosition> {startPosition};
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