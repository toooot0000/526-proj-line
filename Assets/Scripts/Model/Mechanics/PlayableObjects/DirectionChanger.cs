using UnityEngine;
using Utility.Extensions;

namespace Model.Mechanics.PlayableObjects{
    public class DirectionChanger: GameModel, IPlayableObject, ICircleable, ISliceable{
        public RectInt InitGridRectInt{ get; set; }
        public Vector2 targetDirection = Vector2.right;
        public float targetAngle = 0;
        public float size = 0.8f;

        private class CircledEffect: IExecutable{
            private readonly DirectionChanger _changer;
            public CircledEffect(DirectionChanger changer){
                _changer = changer;
            }
            public void Execute(){
                _changer.RotateClockwise();
            }
        }

        private class SlicedEffect: IExecutable{
            private readonly DirectionChanger _changer;
            public SlicedEffect(DirectionChanger changer){
                _changer = changer;
            }
            public void Execute(){
                _changer.RotateCounterClockwise();
            }
        }

        protected DirectionChanger(GameModel parent) : base(parent){ }

        public DirectionChanger(GameModel parent, float initAngle): base(parent){
            targetDirection = Vector2.right.Rotated(initAngle);
            targetAngle = initAngle;
        }

        public void RotateClockwise(){
            targetDirection = targetDirection.Rotated(90);
            targetAngle += 90;
        }

        public void RotateCounterClockwise(){
            targetDirection = targetDirection.Rotated(-90);
            targetAngle -= 90;
        }

        public IExecutable OnCircled(){
            return new CircledEffect(this);
        }

        public IExecutable OnSliced(){
            return new SlicedEffect(this);
        }
    }
}