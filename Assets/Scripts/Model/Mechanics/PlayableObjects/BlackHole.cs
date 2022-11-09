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
        public float range = 5;

        protected BlackHole(GameModel parent) : base(parent){ }

        public BlackHole(GameModel parent, int initRange) : base(parent) {
            range = initRange;
        }
        
        public IExecutable OnCircled() {
            return new CircledEffect() {
                hole = this
            };
        }

        public void Shrink() {
            range--;
            range = Math.Max(range, 0);
        }

        public bool IsDead() => range <= 0;
    }
}