using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tutorials{
    public delegate void TutorialEvent(TutorialBase tutorial);

    /// <summary>
    ///     In world space 
    /// </summary>
    public abstract class TutorialBase : MonoBehaviour{
        private const float HighLightZIndex = -190;
        public string nextTutorialName;
        public float nextDelay = 0.1f;
        public TutorialManager tutorialManager;

        private readonly Dictionary<GameObject, Vector3> _positions = new();
        private int _currentStepIndex;
        protected abstract IStepBase[] Steps{ get; }
        public event TutorialEvent OnLoad;
        public event TutorialEvent OnComplete;

        public virtual void OnLoaded(TutorialManager mng){
            tutorialManager = mng;
            OnLoad?.Invoke(this);
            _currentStepIndex = 0;
            CurrentStep().SetUp(this);
        }

        public virtual void StepComplete(ITutorialControllable controllable){
            CurrentStep().Complete(this);
            MoveToNextStep();
            if (CurrentStep() == null)
                Complete();
            else
                Steps[_currentStepIndex].SetUp(this);
        }

        protected virtual void Complete(){
            OnComplete?.Invoke(this);
            Destroy(gameObject);
        }

        protected void MoveToNextStep(){
            _currentStepIndex = Math.Min(Steps.Length, _currentStepIndex + 1);
        }

        protected IStepBase CurrentStep(){
            return _currentStepIndex >= Steps.Length ? null : Steps[_currentStepIndex];
        }

        public void LiftToFront(GameObject obj, float relative = 0){
            if (obj == null) return;
            _positions[obj] = obj.transform.position;
            obj.transform.position =
                new Vector3(_positions[obj].x, _positions[obj].y, HighLightZIndex + relative);
        }

        public void PutToBack(GameObject obj){
            if (obj == null) return;
            obj.transform.position = _positions[obj];
        }
    }

    public class TutorialContextBase{ }
}