using FireEmblem.Domain.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FireEmblem.MapView
{
    public class MapClickHandler : MonoBehaviour
    {
        [SerializeField] private MapController mapController;
        [SerializeField] private MapGrid grid;
        [SerializeField] private GameObject cursor;

        private MapPosition selectedTile;

        private void Start()
        {
            MoveCursorToPosition(new MapPosition(0,0));
        }

        public void MoveCursorToLocation(InputAction.CallbackContext callbackContext)
        {
            var mousePosition = callbackContext.ReadValue<Vector2>();
            var position = GetGridPosition(mousePosition);
            
            if (position != selectedTile)
            {
                MoveCursorToPosition(position);
            }
        }

        private void MoveCursorToPosition(MapPosition position)
        {
            selectedTile = position;
            mapController.HighlightCell(position);
            grid.MoveObjectToGridPosition(cursor, position);
        }

        public void MoveCursor(InputAction.CallbackContext callbackContext)
        {
            var delta = callbackContext.ReadValue<Vector2>();
            var deltaX = Mathf.RoundToInt(delta.x);
            var deltaY = Mathf.RoundToInt(delta.y);
            var position = new MapPosition(X: selectedTile.X + deltaX, Y: selectedTile.Y + deltaY);
            if (deltaX != 0 || deltaY != 0)
            {
                Debug.Log($"{deltaX}, {deltaY}");
            }
            
            if (position != selectedTile)
            {
                MoveCursorToPosition(position);
            }
        }

        public void SelectCurrentCell(InputAction.CallbackContext callbackContext)
        {
            mapController.SelectCell(selectedTile);
        }

        private MapPosition GetGridPosition(Vector3 mousePosition)
        {
            var gridPlane = new Plane(Vector3.up, grid.transform.position);
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            var pointOnGrid = gridPlane.Raycast(ray, out var point) ? ray.GetPoint(point) : Vector3.zero;
            
            return MapPosition.From3dVector(pointOnGrid);
        }
    }
}