using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Data;
using FireEmblem.MapView.UI;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private TileObjectManager tileObjectManager;
        [SerializeField] private MapGrid mapGrid;
        [SerializeField] private UnitStatsDisplay unitStatsDisplay;
        private List<PlayerUnit> PlayerUnits { get; set; } = new List<PlayerUnit>();
        private List<EnemyUnit> EnemyUnits { get; set; } = new List<EnemyUnit>();

        private BaseUnit _selectedUnit;
        private Dictionary<MapPosition, AccessibleTile> _accessibleTiles;
        private MovementGenerator _movementGenerator;

        private void Awake()
        {
            PlayerUnits = GetComponentsInChildren<PlayerUnit>().ToList();
            EnemyUnits = GetComponentsInChildren<EnemyUnit>().ToList();

            _movementGenerator = new MovementGenerator(mapGrid);
        }

        private void SelectUnit(BaseUnit unit)
        {
            _selectedUnit = unit;
            unitStatsDisplay.DisplayUnit(unit.Unit);
            tileObjectManager.DestroyAll();
            
            _accessibleTiles = _movementGenerator.GenerateAccessibleTiles(unit, EnemyUnits);
            ShowAccessibleTiles(_accessibleTiles);
        }

        public void SelectCell(MapPosition position)
        {
            if (_selectedUnit != null)
            {
                var tileIsAccessible = _accessibleTiles.TryGetValue(position, out var tile);
                if (tileIsAccessible && tile is { Accessibility: TileAccessibility.CanMoveTo })
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
            }
        }

        private void ShowAccessibleTiles(Dictionary<MapPosition, AccessibleTile> accessibleTiles)
        {
            foreach (var position in accessibleTiles.Keys)
            {
                var tile = accessibleTiles[position];
                
                switch (tile.Accessibility)
                {
                    case TileAccessibility.CanMoveTo:
                        tileObjectManager.CreateMoveTile(position);
                        break;
                    case TileAccessibility.CanAttack:
                        tileObjectManager.CreateAttackTile(position);
                        break;
                }
            }
        }

        public void HighlightTile(MapPosition tile)
        {
            if (_selectedUnit != null)
            {
                tileObjectManager.ClearMovePath();
                if (_accessibleTiles.ContainsKey(tile))
                {
                    ShowPathToTile(tile);
                }
            }
        }
        
        public void ShowPathToTile(MapPosition targetPosition)
        {
            var currentPosition = targetPosition;

            var tilesInPath = new List<MapPosition>();
            
            while (currentPosition != null && !currentPosition.Equals(_selectedUnit.Position))
            {
                tilesInPath.Add(currentPosition);
                currentPosition = _accessibleTiles[currentPosition].SourceTiles.FirstOrDefault();
            }

            foreach (var tile in tilesInPath)
            {
                tileObjectManager.CreateMovePathTile(tile);
            }
        }
    }
}