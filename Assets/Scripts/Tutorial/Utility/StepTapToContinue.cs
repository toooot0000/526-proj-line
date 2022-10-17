using System;
using System.Collections.Generic;
using System.Linq;
using Tutorial.Common;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace Tutorial.Utility{
    public class StepTapToContinue : StepConditionToContinue{
        private readonly TutorialText _textMesh;
        private readonly TouchCatcher _catcher;
        private readonly List<GameObject> _highlights = new();

        public StepTapToContinue(TutorialText text, TouchCatcher catcher, GameObject highlight = null): this(text, catcher, new []{highlight}){ }

        private StepTapToContinue(TutorialText text, TouchCatcher catcher, GameObject[] highlights)
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

        public StepTapToContinue(TutorialText text, TouchCatcher catcher, GameObject highlight, StepCallbackDelegate setUp): this(text, catcher, highlight){
            SetUpProcedure = setUp;
        }

        public StepTapToContinue(TutorialText text,  TouchCatcher catcher, StepCallbackDelegate setUp, StepCallbackDelegate cleanUp = null,
            StepCallbackDelegate bind = null, StepCallbackDelegate unbind = null): base(setUp, cleanUp ?? DefaultCleanUp, bind ?? DefaultBind, unbind ?? DefaultUnbind){
            _textMesh = text;
            _catcher = catcher;
        }

        public static void DefaultSetUp(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
            step._textMesh.Enabled = true;
            step._catcher.Enabled = true;
            step.HighlightAll(t);
        }

        public static void DefaultBind(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
            step._catcher.OnTouched += t.StepComplete;
        }

        public static void DefaultCleanUp(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
            step._textMesh.Enabled = false;
            step._catcher.Enabled = false;
            step.LowlightAll(t);
        }

        public static void DefaultUnbind(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
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