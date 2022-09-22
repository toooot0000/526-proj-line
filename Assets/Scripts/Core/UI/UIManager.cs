using System;
 using System.Collections.Generic;
 using UnityEngine;
using Utility;

namespace Core.UI{
     public delegate void UINormalEvent(UIBase ui);
     public class UIManager: MonoBehaviour{
         public const string ResourcesFolder = "UIs/";
         public static UIManager shared;

         private readonly List<UIBase> _uiList = new();

 
         private void Awake(){
             if (shared){
                 Destroy(this);
             }
             shared = this;
         }
 
         public UIBase OpenUI(string uiPrefabName){
             var cur = _uiList.Find(uiBase => uiBase.name == uiPrefabName);
             if( cur != null){
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
             cur.OnClose += RemoveUI;
             StartCoroutine(CoroutineUtility.Delayed(() => cur.Open()));
             return cur;
         }

         public void RemoveUI(UIBase ui){
             _uiList.Remove(ui);
         }


         public void Open(string interfaceName){
             OpenUI(interfaceName);
         }
     }
 }