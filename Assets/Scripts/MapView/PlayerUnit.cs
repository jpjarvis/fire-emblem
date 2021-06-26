using System;
using FireEmblem.Model.Map;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class PlayerUnit : MonoBehaviour
    {
        public MapUnit Unit { get; set; }

        private void OnMouseDown()
        {
            MapManager.Instance.SelectUnit(Unit);
        }
    }
}