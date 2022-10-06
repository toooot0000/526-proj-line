using System.Collections.Generic;
using UI.Interfaces;
using UnityEngine;
using Utility;

namespace UI{
    public delegate void UINormalEvent(UIBase ui);

    public class UIManager : MonoBehaviour{
        public const string ResourcesFolder = "Prefabs/UIs/";
        public static UIManager shared;

        public Shade shade;

        private readonly List<UIBase> _uiList = new();


        private void Awake(){
            if (shared) Destroy(this);
            shared = this;
        }


        private void Start(){
            // OpenUI("UIGameStart");
        }

        public UIBase OpenUI(string uiPrefabName, object arg1 = null){
            var cur = _uiList.Find(uiBase => uiBase.name == uiPrefabName);
            if (cur != null){
                _uiList.Remove(cur);
                _uiList.Add(cur);
                return cur;
            }

            var ui = Resources.Load<GameObject>($"{ResourcesFolder}{uiPrefabName}");
            if (ui == null){
                Debug.LogError($"Unable to find UI resource: {uiPrefabName}");
                return null;
            }

            ui = Instantiate(ui, transform);
            cur = ui.GetComponent<UIBase>();
            if (cur == null){
                Debug.LogError($"UI prefab doesn't have UIBase component! PrefabName = {uiPrefabName}");
                return null;
            }

            _uiList.Add(cur);
            shade.SetActive(true);
            cur.OnClose += RemoveUI;
            StartCoroutine(CoroutineUtility.Delayed(() => cur.Open(arg1)));
            return cur;
        }

        public void RemoveUI(UIBase ui){
            _uiList.Remove(ui);
            if (_uiList.Count == 0) shade.SetActive(false);
        }


        public void Open(string interfaceName){
            OpenUI(interfaceName);
        }
    }
}