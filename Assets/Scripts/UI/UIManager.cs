using System.Collections.Generic;
using Tutorial;
using Tutorial.UI;
using UI.Common;
using UI.Common.Shade;
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
        private readonly List<UIComponent> _uiComponents = new();
        public GameObject uiContainer;

        public event UINormalEvent OnLoadUI; // right after load a ui;
        public event UINormalEvent OnOpenUI; // Right after ui is opened;
        public event UINormalEvent OnCloseUI; // Right after ui is closed;

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

            ui = Instantiate(ui, uiContainer.transform);
            cur = ui.GetComponent<UIBase>();
            OnLoadUI?.Invoke(cur);
            if (cur == null){
                Debug.LogError($"UI prefab doesn't have UIBase component! PrefabName = {uiPrefabName}");
                return null;
            }

            _uiList.Add(cur);
            shade.SetActive(true);
            cur.OnClose += RemoveUI;
            StartCoroutine(CoroutineUtility.Delayed(() => {
                cur.Open(arg1);
                OnOpenUI?.Invoke(cur);
            }));
            return cur;
        }

        public void RemoveUI(UIBase ui){
            OnCloseUI?.Invoke(ui);
            _uiList.Remove(ui);
            if (_uiList.Count == 0) shade.SetActive(false);
        }


        public void Open(string interfaceName){
            OpenUI(interfaceName);
        }

        public void RegisterComponent(UIComponent component){
            _uiComponents.Add(component);
        }

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
            foreach (var comp in _uiComponents){
                comp.HandOverControlTo(tutorial);
            }
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
            foreach (var comp in _uiComponents){
                comp.GainBackControlFrom(tutorial);
            }
        }

        public void HideAllComponents(){
            foreach (var comp in _uiComponents){
                comp.Hide();
            }
        }

        public void ShowAllComponents(){
            foreach (var comp in _uiComponents){
                comp.Show();
            }
        }

        public T GetUI<T>() where T : UIBase{
            return _uiList.Find(b => b.GetType() == typeof(T)) as T;
        }

        public T GetUIComponent<T>() where T : UIComponent{
            return _uiComponents.Find(b => b.GetType() == typeof(T)) as T;
        }
    }
}