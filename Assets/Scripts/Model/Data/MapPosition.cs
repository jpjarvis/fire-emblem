using UnityEngine;

namespace FireEmblem.Model.Data
{
    public class MapPosition
    {
        public int X { get; }
        public int Y { get; }

        public MapPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public bool Equals(MapPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        public static MapPosition FromVector(Vector3 vector) => new MapPosition(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
}