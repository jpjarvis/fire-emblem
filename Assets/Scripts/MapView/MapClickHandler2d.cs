using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapClickHandler2d : MonoBehaviour
    {
        [SerializeField] private MapController mapController;
        [SerializeField] private Map map;
        [SerializeField] private GameObject cursor;

        private MapPosition _tileUnderMouse;
        
        private void Update()
        {
            var position = MapPosition.FromVector(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (position != _tileUnderMouse)
            {
                _tileUnderMouse = position;
                mapController.HighlightCell(position);
                map.MoveObjectToGridPosition(cursor, position);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                mapController.SelectCell(position);
            }
        }
    }
}