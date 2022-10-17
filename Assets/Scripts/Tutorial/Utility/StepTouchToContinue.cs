using System;
using Tutorial.Common;
using UnityEngine;

namespace Tutorial.Utility{
    public class StepTouchToContinue : StepBase{
        private readonly TutorialText _textMesh;
        private readonly TouchCatcher _catcher;
        private readonly GameObject[] _highlights;
        public sealed override Action<TutorialBase, StepBase> BindEvent{ get; protected set; } = null;

        public StepTouchToContinue(TutorialText text, TouchCatcher catcher, GameObject highlight = null){
            _textMesh = text;
            _catcher = catcher;
            _highlights = new []{highlight};
        }

        public StepTouchToContinue(TutorialText text, TouchCatcher catcher, GameObject[] highlights){
            _textMesh = text;
            _catcher = catcher;
            _highlights = highlights;
        }

        public StepTouchToContinue(TutorialText text, TouchCatcher catcher, GameObject highlight, Action<TutorialBase, StepBase> bind): this(text, catcher, highlight){
            BindEvent = bind;
        }

        public override void SetUp(TutorialBase ttr){
            _textMesh.Enable = true;
            _catcher.Enabled = true;
            foreach (var gameObject in _highlights){
                ttr.LiftToFront(gameObject);
            }
            _catcher.OnTouched += ttr.StepComplete;
        }

        public override void Complete(TutorialBase ttr){
            _textMesh.Enable = false;
            _catcher.Enabled = false;
            foreach (var gameObject in _highlights){
                ttr.PutToBack(gameObject);
            }
            _catcher.OnTouched -= ttr.StepComplete;
        }
    }
}