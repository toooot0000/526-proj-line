using System;
using TMPro;
using UnityEngine;

namespace Tutorials{
    [RequireComponent(typeof(Collision2D))]
    public class TouchCatcher : MonoBehaviour{
        public Collider2D col;
        public TextMeshProUGUI mesh;

        public bool Enable{
            set{
                col.enabled = value;
                mesh.enabled = value;
                enabled = value;
            }
            get => enabled;
        }


        private void OnMouseDown(){
            OnTouched?.Invoke();
        }

        public event Action OnTouched;
    }
}