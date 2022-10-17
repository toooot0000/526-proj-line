using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Tutorial.UI{

    public abstract class UITutorialBase: TutorialBase{

        private readonly Dictionary<GameObject, Transform> _parents = new();
        
        public override void LiftToFront(GameObject obj, float relative = 0){
            if (obj == null) return;
            _parents[obj] = obj.transform.parent;
            obj.transform.SetParent(transform, true);
        }

        public override void PutToBack(GameObject obj){
            if (obj == null) return;
            obj.transform.SetParent(_parents[obj]);
        }
    }
}