using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Buff;
using UnityEngine;

namespace Core.DisplayArea.BuffDisplayer{
    public class BuffDisplayer: MonoBehaviour{

        public GameObject itemPrefab;
        
        private readonly List<BuffDisplayerItem> _items = new();
        private Damageable _target;
        public Damageable Target{
            set{
                if (_target != null){
                    _target.OnBuffLayerAdded -= AddBuffLayer;
                    _target.OnBuffLayerRemoved -= RemoveBuffLayer;
                }
                _target = value;
                if (_target != null){
                    _target.OnBuffLayerAdded += AddBuffLayer;
                    _target.OnBuffLayerRemoved += RemoveBuffLayer;
                }
            }
            get => _target;
        }

        private void AddBuffLayer(Game game, Buff buff){
            var itemModel = _items.Find(i => i.Model == buff);
            if (itemModel != null) return;
            var newIem = Instantiate(itemPrefab, transform).GetComponent<BuffDisplayerItem>();
            newIem.Model = buff;
            _items.Add(newIem);
        }

        private void RemoveBuffLayer(Game game, Buff buff){
            if (buff.layer > 0) return;
            var item = _items.Find(i => i.Model == buff);
            Destroy(item.gameObject);
        }
    }
}