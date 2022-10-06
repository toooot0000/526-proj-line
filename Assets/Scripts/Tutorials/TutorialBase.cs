using System;
using System.Collections.Generic;
using Tutorials.SliceBall;
using UnityEngine;

namespace Tutorials{
    public delegate void TutorialEvent(TutorialBase tutorial);

    /// <summary>
    ///     一类特殊的界面，覆盖再所有层级之上。
    /// </summary>
    public abstract class TutorialBase : MonoBehaviour{
        private const float HighLightZIndex = -190;
        public string nextTutorialName;
        public float nextDelay = 0.1f;
        protected TutorialManager tutorialManager;
        
        private readonly Dictionary<GameObject, Vector3> _positions = new();
        protected abstract StepBase[] Steps{ get; }
        private int _currentStepIndex = 0;

        public event TutorialEvent OnLoad;
        public event TutorialEvent OnComplete;

        public virtual void Load(TutorialManager mng){
            tutorialManager = mng;
            OnLoad?.Invoke(this);
            _currentStepIndex = 0;
            CurrentStep().SetUp(this);
        }

        protected virtual void StepComplete(ITutorialControllable controllable){
            CurrentStep().Complete(this);
            MoveToNextStep();
            if (CurrentStep() == null){
                Complete();
            } else
                Steps[_currentStepIndex].SetUp(this);
        }

        protected virtual void Complete(){
            OnComplete?.Invoke(this);
            Destroy(gameObject);
        }

        protected void MoveToNextStep(){
            _currentStepIndex = Math.Min(Steps.Length, _currentStepIndex + 1);
        }
        protected StepBase CurrentStep() => _currentStepIndex >= Steps.Length ? null : Steps[_currentStepIndex];

        public void LiftToFront(GameObject obj, float relative = 0){
            _positions[obj] = obj.transform.position;
            obj.transform.position =
                new Vector3(_positions[obj].x, _positions[obj].y, HighLightZIndex + relative);
        }

        public void PutToBack(GameObject obj){
            obj.transform.position = _positions[obj];
        }
    }

    public class TutorialContextBase{ }
}