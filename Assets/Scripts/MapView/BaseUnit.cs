using FireEmblem.Data;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [SerializeField] private UnitData unitData;

        public Unit Unit { get; set; }

        public MapPosition Position => MapPosition.From3dVector(transform.position);
        
        private void Awake()
        {
            Unit = Unit.Create(unitData);
        }
    }
}