using System.Collections.Generic;
using UI;
using UI.Common.Shade;
using UnityEngine;

namespace Tutorial.UI{
    public class TutorialUIController: MonoBehaviour{
        public UIShade tutorialShade;
        private Dictionary<UIComponent, Transform> _parents;

        public new void LiftToFront(UIComponent uiComponent){
            if (uiComponent == null) return;
            _parents[uiComponent] = uiComponent.transform.parent;
            uiComponent.transform.SetParent(this.transform);
        }
        
        public new void PutToBack(UIComponent uiComponent){
            if (uiComponent == null) return;
            uiComponent.transform.SetParent(_parents[uiComponent]);
        }
    }
}