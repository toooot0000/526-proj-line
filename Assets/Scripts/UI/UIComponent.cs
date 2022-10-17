using Tutorial;
using UnityEngine;

namespace UI{
    public abstract class UIComponent: MonoBehaviour, ITutorialControllable{
        private void Start(){
            UIManager.shared.RegisterComponent(this);
        }

        public virtual void HandOverControlTo(TutorialBase tutorial){ }
        public virtual void GainBackControlFrom(TutorialBase tutorial){ }
        public abstract void Hide();
        public abstract void Show();
    }
}