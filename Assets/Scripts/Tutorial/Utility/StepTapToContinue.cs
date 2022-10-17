using System;
using System.Collections.Generic;
using System.Linq;
using Tutorial.Common;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace Tutorial.Utility{
    public class StepTapToContinue<T> : StepConditionToContinue<T>
    where T: TutorialBase{
        private readonly TutorialText _textMesh;
        private readonly TutorialTapCatcher _catcher;
        private readonly List<GameObject> _highlights = new();

        public StepTapToContinue(TutorialText text, TutorialTapCatcher catcher, GameObject highlight = null): this(text, catcher, new []{highlight}){ }

        private StepTapToContinue(TutorialText text, TutorialTapCatcher catcher, GameObject[] highlights)
            : base(
                setUpProcedure: DefaultSetUp,
                cleanUpProcedure: DefaultCleanUp,
                continueConditionBind: DefaultBind,
                continueConditionUnbind: DefaultUnbind
            ){
            _textMesh = text;
            _catcher = catcher;
            foreach (var obj in highlights){
                if (obj == null) continue;
                _highlights.Add(obj);
            }
        }

        public StepTapToContinue(TutorialText text, TutorialTapCatcher catcher, GameObject highlight, Action<T, StepTapToContinue<T>> setUp): this(text, catcher, highlight){
            SetUpProcedure = (t, s) => {
                setUp(t, (StepTapToContinue<T>)s);
            };
        }

        public StepTapToContinue(TutorialText text,  TutorialTapCatcher catcher, Action<T, StepTapToContinue<T>>  setUp, Action<T, StepTapToContinue<T>>  cleanUp = null,
            Action<T, StepTapToContinue<T>>  bind = null, Action<T, StepTapToContinue<T>>  unbind = null)
            : base(
            (t, s) => { setUp(t, (StepTapToContinue<T>)s); }, 
                cleanUp != null ? (t, s) => { cleanUp(t, (StepTapToContinue<T>)s); } : DefaultCleanUp, 
                bind != null ? (t, s) => { bind(t, (StepTapToContinue<T>)s); } : DefaultBind, 
                unbind != null ? (t, s) => { unbind(t, (StepTapToContinue<T>)s); } : DefaultUnbind
            ){
            _textMesh = text;
            _catcher = catcher;
        }

        public static void DefaultSetUp(T t, StepConditionToContinue<T> s){
            if (s is not StepTapToContinue<T> step) return;
            step._textMesh.Enabled = true;
            step._catcher.Enabled = true;
            step.HighlightAll(t);
        }

        public static void DefaultBind(T t, StepConditionToContinue<T> s){
            if (s is not StepTapToContinue<T> step) return;
            step._catcher.OnTouched += t.StepComplete;
        }

        public static void DefaultCleanUp(T t, StepConditionToContinue<T> s){
            if (s is not StepTapToContinue<T> step) return;
            step._textMesh.Enabled = false;
            step._catcher.Enabled = false;
            step.LowlightAll(t);
        }

        public static void DefaultUnbind(T t, StepConditionToContinue<T> s){
            if (s is not StepTapToContinue<T> step) return;
            step._catcher.OnTouched -= t.StepComplete;
        }

        public void AddHighlightObject(GameObject obj){
            _highlights.Add(obj);
        }

        public void HighlightAll(TutorialBase tutorial){
            foreach (var gObj in _highlights){
                tutorial.LiftToFront(gObj);
            }
        }

        public void LowlightAll(TutorialBase tutorial){
            foreach (var gameObject in _highlights){
                tutorial.PutToBack(gameObject);
            }
        }

        public void SetTextEnabled(bool value){
            _textMesh.Enabled = value;
        }

        public void SetCatcherEnabled(bool value){
            _catcher.Enabled = value;
        }
    }
}