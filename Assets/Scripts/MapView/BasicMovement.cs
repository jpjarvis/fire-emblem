using UnityEngine;
using UnityEngine.InputSystem;

namespace FireEmblem.MapView
{
    public class BasicMovement : MonoBehaviour
    {
        private Vector2 movement;

        private void Update()
        {
            transform.position += 0.1f * new Vector3(movement.x, 0, movement.y);
        }

        public void Move(InputAction.CallbackContext callbackContext)
        {
            movement = callbackContext.ReadValue<Vector2>();
        }
    }
}