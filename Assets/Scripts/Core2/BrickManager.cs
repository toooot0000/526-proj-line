using System;
using UnityEngine;

namespace Core2{

    public enum BrickState{
        BeforeStart,
        Targeting,
        Playing,
        AfterEnd,
    }
    
    public class BrickManager: MonoBehaviour{
        public delegate void StateChangeHandler(BrickManager mng, BrickState nextState);
        public event StateChangeHandler BeforeStateChange;
        public BrickState currentState = BrickState.AfterEnd;
        public delegate void BrickHitHandler(BrickManager mng, Brick brick);
        public event BrickHitHandler OnBrickHit;

        public static BrickManager shared;

        private void Awake(){
            if (shared){
                Destroy(this);
            } else{
                shared = this;
            }
        }

        protected void ChangeState(BrickState nextState){
            BeforeStateChange?.Invoke(this, nextState);
            currentState = nextState;
        }
    }
}