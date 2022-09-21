using UnityEngine;

namespace Core.UI{
    public abstract class UIBase: MonoBehaviour, IUserInterface{
        public abstract void Open();

        public abstract void Close();

        public string Name => "Base";
    }
}