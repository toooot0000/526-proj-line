using System;
using UnityEngine;

namespace Model.Mechanics.PlayableObjects{

    
    public class BlackHole:GameModel, IPlayableObject, ICircleable {

        private class CircledEffect: IExecutable {
            public BlackHole hole;
            public void Execute() {
                hole.Shrink();
            }
        }
        
        public RectInt InitGridPosition{ get; set; }
        public float outRange = 2;
        public readonly float innerRange = 0.5f;

        public event ModelEvent<BlackHole> OnRangeChanged; 

        protected BlackHole(GameModel parent) : base(parent){ }

        public BlackHole(GameModel parent, float initInnerRange, float initOutRange) : base(parent) {
            outRange = initOutRange;
            innerRange = initInnerRange;
        }
        
        public IExecutable OnCircled() {
            return new CircledEffect() {
                hole = this
            };
        }

        public void Shrink() {
            outRange--;
            outRange = Math.Max(outRange, innerRange);
            OnRangeChanged?.Invoke(currentGame, this);
        }

        public bool IsDead() => outRange <= innerRange;
    }
}