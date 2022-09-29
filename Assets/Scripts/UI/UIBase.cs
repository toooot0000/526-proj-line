using UnityEngine;

namespace UI{
    public abstract class UIBase: MonoBehaviour, IUserInterface{
        public virtual void Open(){
            OnOpen?.Invoke(this);
        }

        public virtual void Open(object arg){
            OnOpen?.Invoke(this);
        }

        public virtual void Close(){
            OnClose?.Invoke(this);
        }

        public string Name => "Base";
        public event UINormalEvent OnOpen;
        public event UINormalEvent OnClose;
    }
}