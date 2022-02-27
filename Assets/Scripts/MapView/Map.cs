using System.Collections.Generic;
using System.Linq;
using FireEmblem.MapView.UI;
using FireEmblem.Model.Data;
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
        private IEnumerable<AccessibleTile> _accessibleTiles;
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
            }
        }

        private void ShowAccessibleTiles(IEnumerable<AccessibleTile> accessibleTiles)
        {
            foreach (var tile in accessibleTiles)
            {
                if (tile.Accessibility == TileAccessibility.CanMoveTo)
                {
                    tileObjectManager.CreateMoveTile(tile.Position);
                }

                if (tile.Accessibility == TileAccessibility.CanAttack)
                {
                    tileObjectManager.CreateAttackTile(tile.Position);
                }
            }
        }
    }
}