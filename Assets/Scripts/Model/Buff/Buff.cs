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
        IEnumerable<Buff> GetAllBuffs();
    }


    public abstract class Buff: GameModel, IBuffModifiable{
        public int id;
        public string name;
        public string desc;
        public int layer;

        public event ModelEvent OnBuffLayerRemoved;

        public event ModelEvent OnBuffLayerAdded;

        public event ModelEvent OnBuffCanceled; // equal to remove layer to 0;

        public static IEnumerable<T> GetBuffOfTriggerFrom<T>(IBuffHolder buffable) where T : IBuffTrigger{
            return buffable.GetAllBuffs()?.Where(b => b is T) as IEnumerable<T>;
        }

        public static T GetBuffOfTypeFrom<T>(IBuffHolder buffHolder) where T : Buff{
            return (T)buffHolder.GetAllBuffs().First(b => b is T);
        }

        public static T MakeBuff<T>(GameModel parent, int layer) where T : Buff{
            return typeof(T).GetConstructor(new []{ typeof(GameModel), typeof(int) })?.Invoke(
                new object[]{
                    parent, layer
                }) as T;
        }

        public static string BuffsToString(IBuffHolder buffHolder){
            string ret = "";
            foreach (var buff in buffHolder.GetAllBuffs()){
                ret = $"{ret}, {buff}";
            }
            return $"[{ret}]";
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
        
        /// <summary>
        /// Call in children's constructors
        /// </summary>
        protected static void SetUp(Buff buff){
            buff.id = NameToId[buff.GetBuffName()];
            var info = CsvLoader.TryToLoad("Configs/buffs", buff.id);
            if (info == null) return;
            buff.name = info["name"] as string;
            buff.desc = info["desc"] as string;
        }

        public override string ToString(){
            return $"Buff: [name={name}], [layer={layer.ToString()}]";
        }
    }
}