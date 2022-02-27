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

        private void ShowAccessibleTiles(IEnumerable<AccessibleTile> accessibleTiles)
        {
            foreach (var tile in accessibleTiles)
            {
                tileObjectManager.CreateMoveTile(tile.Position);
            }
        }
    }
}