using System;
using Model.Mechanics.PlayableObjects.CrystalEffects;
using UnityEngine;

namespace Model.Mechanics.PlayableObjects {
    public abstract class CrystalEffect : IResetable{
        public abstract void Execute();
        public abstract void Reset();
    }

    public enum CrystalType{
        LengthenLine,
        FreezeMovable
    }

    public class Crystal: GameModel, IPlayableObject, ISliceable{
        private readonly CrystalEffect _effect;
        public readonly CrystalType type;
        protected Crystal(GameModel parent) : base(parent) { }
        public Crystal(GameModel parent, CrystalType type, object arg): base(parent){
            _effect = type switch{
                CrystalType.LengthenLine => new CrystalEffectLengthenLine(){ value = (float)arg },
                CrystalType.FreezeMovable => new CrystalEffectChangeMovableSpeed(){ factor = (float)arg},
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            this.type = type;
        }

        public RectInt InitGridRectInt { get; set; }

        public IExecutable OnSliced(){
            return _effect;
        }
    }
}