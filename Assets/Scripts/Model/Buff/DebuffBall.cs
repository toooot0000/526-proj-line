using System;
using Model.DebuffBallEffects;
using Utility.Loader;

namespace Model.Buff{
    public class DebuffBall: Ball{

        public DebuffBallEffectBase debuffEffect;
        
        public DebuffBall(GameModel parent) : base(parent){ }

        public DebuffBall(Gear parent, int id) : base(parent, id){
            
            var ball = CsvLoader.TryToLoad("Configs/balls", id);
            if (ball == null) return;
            var debuffEffectStr = ball["debuff_effect"] as string;
            if (!string.IsNullOrEmpty(debuffEffectStr)){
                var parts = debuffEffectStr.Split(";");
                var className = parts[0];
                debuffEffect = Activator.CreateInstance(Type.GetType($"Model.DebuffBallEffects.{className}", true),
                    new object[]{ parts[1..] }) as DebuffBallEffectBase;
            }
        }
        
        
        
    }
}