using System;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Start()
        {
            var map = FindObjectOfType<MapController>();
            
            map.SelectCell(new MapPosition(0, 0));
        }
    }
}