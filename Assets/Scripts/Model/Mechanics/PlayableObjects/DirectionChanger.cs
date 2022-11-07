using UnityEngine;
using Utility.Extensions;

namespace Model.Mechanics.PlayableObjects{
    public class DirectionChanger: GameModel, IPlayableObject{
        public RectInt InitGridPosition{ get; set; }
        public Vector2 targetDirection;


        protected DirectionChanger(GameModel parent) : base(parent){ }

        public DirectionChanger(GameModel parent, Vector2 initDirection): base(parent){
            targetDirection = initDirection;
        }

        public void RotateClockwise(){
            targetDirection = targetDirection.Rotated(90);
        }

        public void RotateCounterClockwise(){
            targetDirection = targetDirection.Rotated(-90);
        }
    }
}