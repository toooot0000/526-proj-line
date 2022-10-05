using System.Collections.Generic;
using UnityEngine;

namespace Tutorials{

    public delegate void TutorialEvent(TutorialBase tutorial);
    
    /// <summary>
    /// 一类特殊的界面，覆盖再所有层级之上。
    /// </summary>
    public abstract class TutorialBase: MonoBehaviour{
        public event TutorialEvent OnLoad;
        public event TutorialEvent OnComplete;

        public string nextTutorialName = null;
        
        public float nextDelay = 0.1f;

        private readonly Dictionary<GameObject, Vector3> _positions = new();

        private const float HighLightZIndex = -190;

        public virtual void Load(TutorialContextBase context){
            OnLoad?.Invoke(this);
        }

        public virtual void Complete(){
            OnComplete?.Invoke(this);
            Destroy(gameObject);
        }

        protected void LiftToFront(GameObject obj, float relative = 0){
            _positions[obj] = obj.transform.position;
            obj.transform.position =
                new Vector3(_positions[obj].x, _positions[obj].y, HighLightZIndex + relative);
        }

        protected void PutToBack(GameObject obj){
            obj.transform.position = _positions[obj];
        }
    }
    
    public class TutorialContextBase {}
}