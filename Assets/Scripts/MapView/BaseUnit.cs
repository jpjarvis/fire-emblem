using FireEmblem.Model.Combat;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [SerializeField] private UnitData unitData;

        public Unit Unit { get; set; }

        public MapPosition Position => new MapPosition
        {
            X = Mathf.FloorToInt(transform.localPosition.x),
            Y = Mathf.FloorToInt(transform.localPosition.y)
        };
        
        private void Awake()
        {
            Unit = Unit.Create(unitData);
        }
    }
}