using System;
using UnityEngine;


namespace Core2{

    [RequireComponent(typeof(Collider2D))]
    public class TouchTrigger: MonoBehaviour{
        public delegate void TouchEventHandler(TouchTrigger trigger);

        public event TouchEventHandler PointerDown;
        public event TouchEventHandler PointerUp;

        private void OnMouseDown(){
            PointerDown?.Invoke(this);
        }

        private void OnMouseUp(){
            PointerUp?.Invoke(this);
        }
    }
}