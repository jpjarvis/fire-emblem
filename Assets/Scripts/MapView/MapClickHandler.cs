using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapClickHandler : MonoBehaviour
    {
        [SerializeField] private Map map;

        private MapPosition _tileUnderMouse;
        
        private void Update()
        {
            var position = MapPosition.FromVector(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (!position.Equals(_tileUnderMouse))
            {
                _tileUnderMouse = position;
                map.HighlightTile(position);
            }
            
            map.HighlightTile(position);
            
            if (Input.GetMouseButtonDown(0))
            {
                map.SelectCell(position);
            }
        }
    }
}