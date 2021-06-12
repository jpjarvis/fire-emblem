using System;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class MapPosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(MapPosition other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}