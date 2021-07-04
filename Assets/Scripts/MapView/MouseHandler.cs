using System;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MouseHandler : MonoBehaviour
    {
        public Action ActionOnClick { get; set; } = () => { };
        public Action ActionOnMouseOver { get; set; } = () => { };

        private void OnMouseDown()
        {
            ActionOnClick();
        }

        private void OnMouseOver()
        {
            ActionOnMouseOver();
        }
    }
}