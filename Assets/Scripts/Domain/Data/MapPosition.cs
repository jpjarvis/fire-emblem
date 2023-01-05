using UnityEngine;

namespace FireEmblem.Domain.Data
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
            if (other is not MapPosition otherMapPosition)
                return false;
            
            return X == otherMapPosition.X && Y == otherMapPosition.Y;
        }
        
        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static MapPosition FromVector(Vector3 vector) => new(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        public static MapPosition From3dVector(Vector3 vector) => new(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.z));

        public Vector3Int ToVector()
        {
            return new Vector3Int(X, Y, 0);
        }
    }
}