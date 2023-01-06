using FireEmblem.Data;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class UnitObject : MonoBehaviour
    {
        [SerializeField] private UnitData unitData;

        [SerializeField] private Allegiance allegiance;

        public Unit Unit { get; private set; }
        
        public MapPosition Position => MapPosition.From3dVector(transform.position);
        
        private void Awake()
        {
            Unit = Unit.Create(unitData, allegiance);
        }
    }
}