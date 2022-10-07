using UnityEngine;

namespace UI.Interfaces{
    public abstract class UIBase : MonoBehaviour{
        public string Name => "Base";
        // public virtual string PrefabName{ get; } = "UIxxxxx";
        //
        // public static T Open<T, P>(P parameters) where T: UIBase{
        //     return UIManager.shared.OpenUI(PrefabName, parameters) as T;
        // }

        public virtual void Open(object nextStageChoice){
            OnOpen?.Invoke(this);
        }

        public virtual void Close(){
            OnClose?.Invoke(this);
        }

        public event UINormalEvent OnOpen;
        public event UINormalEvent OnClose;
    }
}