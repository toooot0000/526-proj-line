using System;
using UnityEngine;

namespace Core.PlayArea.BlackHoles{
    public class OuterRange: PlayableObjectViewBase, ISliceableView{
        public float accelerateFactor = 0.1f;

        public float Range{
            set => transform.localScale = new Vector3(value, value, value);
        }

        private void OnTriggerStay2D(Collider2D col){
            var suckable = col.GetComponent<IForceableView>();
            if (suckable == null) return;
            var direction = (Vector2)transform.position - (Vector2)col.transform.position ;
            direction *= direction.magnitude * accelerateFactor;
            suckable.Acceleration = direction;
        }

        private void OnTriggerExit2D(Collider2D col){
            var suckable = col.GetComponent<IForceableView>();
            if (suckable == null) return;
            suckable.Acceleration = Vector2.zero;
        }

        public void OnSliced(){ }
    }
}