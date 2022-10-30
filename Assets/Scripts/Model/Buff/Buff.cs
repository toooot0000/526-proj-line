using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utility.Loader;

namespace Model.Buff{

    

    public interface IBuffModifiable{ }

    public interface IBuffHolder: IBuffModifiable{
        void AddBuffLayer<TBuff>(int layer) where TBuff: Buff;
        void RemoveBUffLayer<TBuff>(int layer) where TBuff : Buff;
        Buff[] GetAllBuffs();
    }


    public abstract class Buff: GameModel, IBuffModifiable{
        public int id;
        public string name;
        public string desc;
        public int layer;

        public event ModelEvent OnBuffLayerRemoved;

        public event ModelEvent OnBuffLayerAdded;

        public event ModelEvent OnBuffCanceled; // equal to remove layer to 0;

        public static T[] GetBuffOfTriggerFrom<T>(IBuffHolder buffable) where T : IBuffTrigger{
            List<T> ret = new();
            foreach (var buff in buffable.GetAllBuffs()){
                if(buff is not T trigger) continue;
                ret.Add(trigger);
            }
            return ret.ToArray();
        }

        public static T GetBuffOfTypeFrom<T>(IBuffHolder buffHolder) where T : Buff{
            return buffHolder.GetAllBuffs().First(b => b is T) as T;
        }

        public static T MakeBuff<T>(GameModel parent, int layer) where T : Buff{
            return typeof(T).GetConstructor(new []{ typeof(GameModel), typeof(int) })?.Invoke(
                new object[]{
                    parent, layer
                }) as T;
        }

        private static Dictionary<string, int> _nameToId = null;

        private static Dictionary<string, int> NameToId{
            get{
                if (_nameToId == null){
                    MakeNameToIdDict();                    
                }
                return _nameToId;
            }
        }

        private static void MakeNameToIdDict(){
            _nameToId = new();
            var table = CsvLoader.Load("Configs/buffs");
            foreach (var pair in table){
                _nameToId[(pair.Value["name"] as string)!] = pair.Key;
            }
        }

        protected Buff(GameModel parent) : base(parent){ }

        protected Buff(GameModel parent, int layer) : base(parent){
             
            this.layer = layer;
        }

        public void RemoveLayer(int layerNum){
            layer = Math.Max(0, layer - layerNum);
            if (layer == 0){
                OnBuffCanceled?.Invoke(currentGame, this);
            } else{
                OnBuffLayerRemoved?.Invoke(currentGame, this);
            }
        }

        public void AddLayer(int layerNum){
            layer += layerNum;
            OnBuffLayerAdded?.Invoke(currentGame, this);
        }

        protected abstract string GetBuffName();

        protected void SetUp(){
            id = NameToId[GetBuffName()];
            var info = CsvLoader.TryToLoad("Configs/buffs", id);
            if (info == null) return;
            name = info["name"] as string;
            desc = info["desc"] as string;
        }

    }
}