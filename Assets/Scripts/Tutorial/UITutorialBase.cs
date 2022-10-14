using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial{
    public abstract class UITutorialBase: TutorialBase{

        private Dictionary<GameObject, Transform> _parents;

        // public new void LiftToFront(GameObject obj, float relative = 0){
        //     if (obj == null) return;
        //     _parents[obj] = obj.transform.parent.transform;
        //     obj.transform.position =
        //         new Vector3(_positions[obj].x, _positions[obj].y, HighLightZIndex + relative);
        // }
        //
        // public new void PutToBack(GameObject obj){
        //     if (obj == null) return;
        //     obj.transform.position = _positions[obj];
        // }
    }
}