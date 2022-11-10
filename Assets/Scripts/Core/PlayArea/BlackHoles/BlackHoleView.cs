using System;
using Model;
using Model.Mechanics.PlayableObjects;
using UnityEngine;
using Utility;

namespace Core.PlayArea.BlackHoles{
    public class BlackHoleView: PlayableObjectViewWithModel<BlackHole>{

        public InnerRange inner;
        public OuterRange outer;
        public ShrinkingRange shrinkingRange;
        private BlackHole _model;
        public override BlackHole Model {
            set{
                _model = value;
                outer.Range = value.outRange;
                inner.Range = value.innerRange;
                shrinkingRange.startValue = new Vec3Wrapper(outer.transform.localScale);
                shrinkingRange.endValue = new Vec3Wrapper(inner.transform.localScale);
                shrinkingRange.Play();
            }
            get => _model;
        }

        public void OnCircled() {
            Model.OnCircled().Execute();
            outer.Range = Model.outRange;
            shrinkingRange.startValue = new Vec3Wrapper(outer.transform.localScale);
            if(Model.IsDead()) Die();
        }

        public void Die(){
            gameObject.SetActive(false);
        }
    }
}