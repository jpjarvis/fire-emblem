using System;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class ClickHandler : MonoBehaviour
    {
        public Action OnClick { get; set; }

        public void OnMouseDown()
        {
            OnClick();
        }
    }
}