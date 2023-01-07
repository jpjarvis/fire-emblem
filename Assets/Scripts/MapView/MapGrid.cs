using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    [RequireComponent(typeof(Grid))]
    public class MapGrid : MonoBehaviour
    {
        private Grid grid;

        private void Awake()
        {
            grid = GetComponent<Grid>();
        }
        
        public void MoveObjectToGridPosition(GameObject objectToMove, MapPosition position)
        {
            objectToMove.transform.position =
                grid.GetCellCenterLocal(new Vector3Int(position.X, position.Y, 0));
        }

        public GameObject InstantiateAtGridPosition(GameObject prefab, MapPosition position)
        {
            var instantiatedObject = Instantiate(prefab, transform);
            MoveObjectToGridPosition(instantiatedObject, position);
            return instantiatedObject;
        }
    }
}