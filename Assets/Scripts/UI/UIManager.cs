using System.Collections.Generic;
using Tutorial;
using Tutorials;
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
        public readonly List<UIBase> uiList = new();
        private bool _isInTutorial = false;
        public readonly List<UIComponent> uiComponents = new();
        public GameObject uiContainer;

        private void Awake(){
            if (shared) Destroy(this);
            shared = this;
        }

        public UIBase OpenUI(string uiPrefabName, object arg1 = null){
            var cur = uiList.Find(uiBase => uiBase.name == uiPrefabName);
            if (cur != null){
                uiList.Remove(cur);
                uiList.Add(cur);
                return cur;
            }

            var ui = Resources.Load<GameObject>($"{ResourcesFolder}{uiPrefabName}");
            if (ui == null){
                Debug.LogError($"Unable to find UI resource: {uiPrefabName}");
                return null;
            }

            ui = Instantiate(ui, uiContainer.transform);
            cur = ui.GetComponent<UIBase>();
            if (cur == null){
                Debug.LogError($"UI prefab doesn't have UIBase component! PrefabName = {uiPrefabName}");
                return null;
            }

            uiList.Add(cur);
            shade.SetActive(true);
            cur.OnClose += RemoveUI;
            StartCoroutine(CoroutineUtility.Delayed(() => cur.Open(arg1)));
            return cur;
        }

        public void RemoveUI(UIBase ui){
            uiList.Remove(ui);
            if (uiList.Count == 0) shade.SetActive(false);
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

        public T GetUI<T>() where T : UIBase{
            return uiList.Find(b => b.GetType() == typeof(T)) as T;
        }

        public T GetUIComponent<T>() where T : UIComponent{
            return uiComponents.Find(b => b.GetType() == typeof(T)) as T;
        }
    }
}