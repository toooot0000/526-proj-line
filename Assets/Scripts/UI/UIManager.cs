using System.Collections.Generic;
using Tutorials;
using UI.Common;
using UI.Interfaces;
using UnityEngine;
using Utility;

namespace UI{
    public delegate void UINormalEvent(UIBase ui);

    public class UIManager : MonoBehaviour, ITutorialControllable{

        public const float UITransitionTime = 0.2f;
        
        private const string ResourcesFolder = "Prefabs/UIs/";
        public static UIManager shared;
        public UIShade shade;
        private readonly List<UIBase> _uiList = new();
        private bool _isInTutorial = false;
        public readonly List<UIComponent> uiComponents = new();

        private void Awake(){
            if (shared) Destroy(this);
            shared = this;
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

        public void RegisterComponent(UIComponent component){
            uiComponents.Add(component);
        }

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
            foreach (var comp in uiComponents){
                comp.HandOverControlTo(tutorial);
            }
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
            foreach (var comp in uiComponents){
                comp.GainBackControlFrom(tutorial);
            }
        }

        public void HideAllComponents(){
            foreach (var comp in uiComponents){
                comp.Hide();
            }
        }

        public void ShowAllComponents(){
            foreach (var comp in uiComponents){
                comp.Show();
            }
        }
    }
}