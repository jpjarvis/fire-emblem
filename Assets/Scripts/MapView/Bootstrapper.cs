using System;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Start()
        {
            var map = FindObjectOfType<Map>();
            
            map.SelectCell(new MapPosition(0, 0));
        }
    }
}