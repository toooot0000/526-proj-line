using System.Collections.Generic;
using Utility.Loader;

namespace Model.Buff{

    public interface IBuffable{
        void AddBuff(Buff buff);
        Buff[] GetBuff();
        
    }
    
    public interface IBuffTrigger{}

    public interface IBuffTriggerOnTurnBegin: IBuffTrigger{
        void OnTurnBegin(IBuffable buffable);
    }

    public interface IBuffTriggerOnTurnEnd: IBuffTrigger{
        void OnTurnEnd(IBuffable buffable);
    }

    public interface IBuffTriggerOnBallHit<in T>: IBuffTrigger where T: Ball{
        void OnBallHit(IBuffable buffable, T ball);
    }

    public interface IBuffTriggerOnInputStart : IBuffable{
        void OnInputStart(IBuffable buffable);
    }

    public abstract class Buff: GameModel{
        public int id;
        public string name;
        public string desc;
        public int layer;

        protected Buff(GameModel parent, int id, int layer) : base(parent){
            this.id = id;
            this.layer = layer;
            var info = CsvLoader.TryToLoad("Configs/buffs", id);
            if (info == null) return;
            name = info["name"] as string;
            name = info["desc"] as string;
        }
    }
}