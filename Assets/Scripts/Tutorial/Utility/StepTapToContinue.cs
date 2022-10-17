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
        
        public StepTapToContinue(TutorialText text, TouchCatcher catcher, StepCallbackDelegate setUp): this(text, catcher){
            SetUpProcedure = setUp;
        }

        public StepTapToContinue(TutorialText text, StepCallbackDelegate setUp, StepCallbackDelegate cleanUp = null,
            StepCallbackDelegate bind = null, StepCallbackDelegate unbind = null): base(setUp, cleanUp, bind, unbind){
            _textMesh = text;
            _catcher = null;
        }

        private static void DefaultSetUp(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
            step._textMesh.Enabled = true;
            foreach (var gObj in step._highlights){
                t.LiftToFront(gObj);
            }
        }

        private static void DefaultBind(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
            step._catcher.Enabled = true;
            step._catcher.OnTouched += t.StepComplete;
        }

        private static void DefaultCleanUp(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
            step._textMesh.Enabled = false;
            step._catcher.Enabled = false;
            foreach (var gameObject in step._highlights){
                t.PutToBack(gameObject);
            }
        }

        private static void DefaultUnbind(TutorialBase t, StepBase s){
            if (s is not StepTapToContinue step) return;
            step._catcher.OnTouched -= t.StepComplete;
        }

        public void AddHighlightObject(GameObject obj){
            _highlights.Add(obj);
        }
    }
}