using System;
using Model.EnemySpecialAttacks;
using Model.Mechanics.PlayableObjects.MineEffects;
using UnityEngine;

namespace Model.Mechanics.PlayableObjects{
    
    public abstract class MineEffect: IExecutable{
        public abstract void Execute();

        public static MineEffect Make(string name, string[] args) {
            return Activator.CreateInstance(Type.GetType($"Model.Mechanics.PlayableObjects.MineEffects.{name}", true),
                    new object[]{ args[1..] }) as
                MineEffect;
        }
    }

    internal class CircledEffect: IExecutable{
        public Mine mine;
        public void Execute(){
            GameManager.shared.game.playArea.RemovePlayableObject(mine);
        }
    }
    
    public class Mine: GameModel, IPlayableObject, IMovable, ICircleable, ISliceable{
        
        public RectInt InitGridRectInt{ get; set; }
        public MineEffect effect;
        public float speed = 2;
        public float size = 1;
        public float Velocity{ get => speed; set => speed = value; }
        public float VelocityMultiplier{ get; set; } = 1;

        public Mine(GameModel parent, RectInt rectInt) : base(parent) {
            InitGridRectInt = rectInt;
        }
        
        public IExecutable OnCircled(){
            return new CircledEffect(){
                mine = this
            };
        }

        public IExecutable OnSliced(){
            ((PlayArea)parent).RemovePlayableObject(this);
            return effect;
        }

    }
}