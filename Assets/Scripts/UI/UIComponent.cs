using Tutorials;
using UnityEngine;

namespace UI{
    public abstract class UIComponent: MonoBehaviour, ITutorialControllable{
        private void Start(){
            UIManager.shared.RegisterComponent(this);
        }
        public abstract void HandOverControlTo(TutorialBase tutorial);
        public abstract void GainBackControlFrom(TutorialBase tutorial);
        public abstract void Hide();
        public abstract void Show();
    }
}