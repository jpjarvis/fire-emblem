using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapClickHandler3d : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private MapGrid mapGrid;
        [SerializeField] private GameObject cursor;

        private MapPosition tileUnderMouse;
        
        private void Update()
        {
            var position = GetGridPosition();

            if (!position.Equals(tileUnderMouse))
            {
                tileUnderMouse = position;
                map.HighlightTile(position);
                mapGrid.MoveObjectToGridPosition(cursor, position);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                map.SelectCell(position);
            }
        }

        private MapPosition GetGridPosition()
        {
            var gridPlane = new Plane(Vector3.up, mapGrid.transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var pointOnGrid = gridPlane.Raycast(ray, out var point) ? ray.GetPoint(point) : Vector3.zero;
            
            return MapPosition.From3dVector(pointOnGrid);
        }
    }
}