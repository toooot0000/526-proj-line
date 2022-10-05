using UnityEngine;

namespace UI.Interfaces {
    public abstract class UIBase : MonoBehaviour {
        public string Name => "Base";

        public virtual void Open(object arg) {
            OnOpen?.Invoke(this);
        }

        public virtual void Close() {
            OnClose?.Invoke(this);
        }

        public event UINormalEvent OnOpen;
        public event UINormalEvent OnClose;
    }
}