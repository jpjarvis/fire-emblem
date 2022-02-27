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
        
        public override bool Equals(object other)
        {
            if (!(other is MapPosition otherMapPosition))
                return false;
            
            return X == otherMapPosition.X && Y == otherMapPosition.Y;
        }
        
        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static MapPosition FromVector(Vector3 vector) => new MapPosition(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
}