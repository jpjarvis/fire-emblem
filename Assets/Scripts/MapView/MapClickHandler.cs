using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapClickHandler : MonoBehaviour
    {
        [SerializeField] private Map map;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log(position);
                map.SelectCell(new MapPosition
                {
                    X = Mathf.FloorToInt(position.x),
                    Y = Mathf.FloorToInt(position.y)
                });
            }
        }
    }
}