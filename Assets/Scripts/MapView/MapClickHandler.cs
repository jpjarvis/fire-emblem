using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapClickHandler : MonoBehaviour
    {
        [SerializeField] private MapController mapController;
        [SerializeField] private MapGrid grid;
        [SerializeField] private GameObject cursor;

        private MapPosition tileUnderMouse;
        
        private void Update()
        {
            var position = GetGridPosition();

            if (position != tileUnderMouse)
            {
                tileUnderMouse = position;
                mapController.HighlightCell(position);
                grid.MoveObjectToGridPosition(cursor, position);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                mapController.SelectCell(position);
            }
        }

        private MapPosition GetGridPosition()
        {
            var gridPlane = new Plane(Vector3.up, grid.transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var pointOnGrid = gridPlane.Raycast(ray, out var point) ? ray.GetPoint(point) : Vector3.zero;
            
            return MapPosition.From3dVector(pointOnGrid);
        }
    }
}