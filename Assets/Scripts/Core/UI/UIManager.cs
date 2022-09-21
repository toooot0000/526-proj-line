using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI{
    public class UIManager: MonoBehaviour{
        public static UIManager shared;

        private List<UIBase> _uiList;

        private void Awake(){
            if (shared){
                Destroy(this);
            }
            shared = this;
        }

        public void OpenUI(string uiName){
            var cur = _uiList.Find(uiBase => uiBase.name == uiName);
            if( cur != null){
                _uiList.Remove(cur);
                _uiList.Add(cur);
            }
            
        }
    }
}