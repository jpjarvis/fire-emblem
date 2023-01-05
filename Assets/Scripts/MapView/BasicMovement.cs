using UnityEngine;

namespace FireEmblem.MapView
{
    public class BasicMovement : MonoBehaviour
    {
        private void Update()
        {
            transform.position += 0.1f * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
    }
}