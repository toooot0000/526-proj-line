using System;
using TMPro;
using UnityEngine;

namespace Tutorial{
    [RequireComponent(typeof(Collision2D))]
    public class TutorialTapCatcher : MonoBehaviour{
        public Collider2D col;
        public TextMeshProUGUI mesh;


        private void Awake(){
            Enabled = false;
        }

        /// <summary>
        /// Controlling show/hide
        /// </summary>
        public bool Enabled{
            set{
                if(col != null) col.enabled = value;
                mesh.enabled = value;
                enabled = value;
            }
            get => enabled;
        }

        public void OnMouseUp(){
            OnTouched?.Invoke(null);
        }

        public event TutorialControllableEvent OnTouched;
    } 
}