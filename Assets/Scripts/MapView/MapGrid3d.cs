using System;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    [RequireComponent(typeof(Grid))]
    public class MapGrid3d : MapGrid
    {
        private Grid grid;

        private void Awake()
        {
            grid = GetComponent<Grid>();
        }

        public override IMapTile GetTileAt(MapPosition mapPosition)
        {
            return new FakeMapTile();
        }

        public override void MoveObjectToGridPosition(GameObject objectToMove, MapPosition position)
        {
        }

        public override GameObject InstantiateAtGridPosition(GameObject prefab, MapPosition position)
        {
            var instance = Instantiate(prefab, transform);
            instance.transform.position = grid.GetCellCenterLocal(position.ToVector());
            return instance;
        }

        private class FakeMapTile : IMapTile
        {
            public bool IsTraversable => true;
        }
    }
}