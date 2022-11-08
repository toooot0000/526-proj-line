using UnityEngine;

namespace Model.Mechanics.PlayableObjects{
    
    public abstract class MineEffect: IExecutable{
        public abstract void Execute();
    }

    internal class CircledEffect: IExecutable{
        private static CircledEffect _shared = null;
        public static CircledEffect Shared => _shared ??= new CircledEffect();
        public PlayArea playArea;
        public Mine mine;
        public void Execute(){
            playArea.RemovePlayableObject(mine);
        }
    }
    
    public class Mine: GameModel, IPlayableObject, IMovable, ICircleable, ISliceable{
        
        public RectInt InitGridPosition{ get; set; }
        public MineEffect effect;
        public float speed = 2;
        public float size = 1;
        public float Velocity{ get => speed; set => speed = value; }
        public float VelocityMultiplier{ get; set; } = 1;

        public Mine(GameModel parent, RectInt rectInt) : base(parent) {
            InitGridPosition = rectInt;
        }
        
        public IExecutable OnCircled(){
            var ret = CircledEffect.Shared;
            ret.mine = this;
            ret.playArea = (PlayArea)parent;
            return ret;
        }

        public IExecutable OnSliced(){
            return effect;
        }

    }
}