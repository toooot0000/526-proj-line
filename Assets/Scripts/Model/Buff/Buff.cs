using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using Utility.Loader;

namespace Model.Buff{
    public interface IBuffModifiable{ }

    public interface IBuffHolder: IBuffModifiable{
        void AddBuffLayer<TBuff>(int layer) where TBuff: Buff;
        void RemoveBuffLayer<TBuff>(int layer) where TBuff : Buff;
        /// <summary>
        /// Get a stream of all buffs;
        /// </summary>
        /// <returns>Null if nothing!</returns>
        IEnumerable<Buff> GetAllBuffs();
    }
    
    public abstract class Buff: GameModel, IBuffModifiable{
        public int id;
        public string name;
        public string desc;
        public int layer;
        public IBuffHolder holder;
        public string icon;
        public string display;
        public string className;

        public event ModelEvent<Buff> OnBuffLayerRemoved;

        public event ModelEvent<Buff> OnBuffLayerAdded;

        public event ModelEvent<Buff> OnBuffCanceled; // equal to remove layer to 0;

        public static IEnumerable<T> GetBuffOfTriggerFrom<T>(IBuffHolder buffable) where T : IBuffTrigger{
            var allBuffs = buffable.GetAllBuffs();
            if(allBuffs == null) yield break;
            foreach (var buff in allBuffs){
                if(buff is not T typed) continue;
                yield return typed;
            }
        }

        public static T GetBuffOfTypeFrom<T>(IBuffHolder buffHolder) where T : Buff{
            try{
                return (T)buffHolder.GetAllBuffs()?.First(b => b is T);
            } catch{
                return null;
            }
        }

        public static T MakeBuff<T>(GameModel parent, int layer) where T : Buff{
            var ret = typeof(T).GetConstructor(new []{ typeof(GameModel), typeof(int) })?.Invoke(
                new object[]{
                    parent, layer
                }) as T;
            SetUp(ret);
            return ret;
        }
        
        /// <summary>
        /// Create a buff using the class name of a buff
        /// </summary>
        /// <param name="className">The class name in Buffs folder, the unique identifier</param>
        /// <param name="parent">The buff holder</param>
        /// <param name="layer">The layer</param>
        /// <returns></returns>
        public static Buff MakeBuffByClassName(string className, GameModel parent, int layer){
            var ret = Activator.CreateInstance(Type.GetType($"Model.Buff.Buffs.{className}", true),
                new object[]{ parent, layer }) as Buff;
            SetUp(ret);
            return ret;
        }

        public static Buff MakeBuffByBuffName(string buffName, GameModel parent, int layer){
            var className = CsvLoader.TryToLoad("Configs/buffs", NameToId[buffName])["class_name"];
            return className == null ? null : MakeBuffByClassName(className as string, parent, layer);
        }
        

        public static string BuffsToString(IBuffHolder buffHolder){
            string ret = "";
            foreach (var buff in buffHolder.GetAllBuffs()){
                ret = $"{ret}, {buff}";
            }
            return $"[{ret}]";
        }

        private static Dictionary<string, int> _nameToId = null;

        public static Dictionary<string, int> NameToId{
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

        protected Buff(GameModel parent, int layer) : base(parent){
            holder = parent as IBuffHolder;
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
        
        private static void SetUp(Buff buff){
            buff.id = NameToId[buff.GetBuffName()];
            var info = CsvLoader.TryToLoad("Configs/buffs", buff.id);
            if (info == null) return;
            buff.name = info["name"] as string;
            buff.desc = info["desc"] as string;
            buff.icon = info["icon"] as string;
            buff.display = info["display_name"] as string;
            buff.className = info["class_name"] as string;
        }

        public override string ToString(){
            return $"Buff: [name={name}], [layer={layer.ToString()}]";
        }

        public string ToDetailString(){
            return $"Name: {display}\nLayer: {layer.ToString()}\nDetail: {desc}";
        }

        public Sprite IconSprite => Resources.Load<Sprite>(icon);
    }
}