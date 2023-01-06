using FireEmblem.Data;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [SerializeField] private UnitData unitData;

        protected abstract Allegiance Allegiance { get; }
        
        public Unit Unit { get; private set; }
        
        public MapPosition Position => MapPosition.From3dVector(transform.position);
        
        private void Awake()
        {
            Unit = Unit.Create(unitData, Allegiance);
        }
    }
}