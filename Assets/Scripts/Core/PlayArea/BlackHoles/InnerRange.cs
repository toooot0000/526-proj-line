using System;
using UnityEngine;

namespace Core.PlayArea.BlackHoles{
    public class InnerRange: PlayableObjectViewBase, ISliceableView, ICircleableView{
        public BlackHoleView parent;
        public float Range{
            set => transform.localScale = new Vector3(value, value, value);
        }

        private void OnCollisionEnter2D(Collision2D col){
            var suckable = col.collider.GetComponent<IBlackHoleSuckableView>();
            suckable?.OnSucked();
        }

        public void OnCircled(){
            parent.OnCircled();
        }

        public void OnSliced(){ }
    }
}