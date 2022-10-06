using System;
using TMPro;
using UnityEngine;

namespace Tutorials{
    [RequireComponent(typeof(Collision2D))]
    public class TouchCatcher : MonoBehaviour{
        public Collider2D col;
        public TextMeshProUGUI mesh;
        
        /// <summary>
        /// Controlling show/hide
        /// </summary>
        public bool Enabled{
            set{
                col.enabled = value;
                mesh.enabled = value;
                enabled = value;
            }
            get => enabled;
        }


        private void OnMouseUp(){
            OnTouched?.Invoke(null);
        }

        public event TutorialControllableEvent OnTouched;
    } 
}