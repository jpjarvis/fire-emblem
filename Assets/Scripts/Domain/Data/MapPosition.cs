using UnityEngine;

namespace FireEmblem.Domain.Data
{
    public record MapPosition(int X, int Y)
    {
        public static MapPosition FromVector(Vector3 vector) => new(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        public static MapPosition From3dVector(Vector3 vector) => new(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.z));
    }
}