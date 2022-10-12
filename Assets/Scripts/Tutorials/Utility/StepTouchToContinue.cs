using TMPro;
using Tutorials.Common;
using UnityEngine;

namespace Tutorials.Utility{
    public class StepTouchToContinue : IStepBase{
        private readonly TutorialText _textMesh;
        private readonly TouchCatcher _catcher;
        private readonly GameObject[] _highlights;
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

        public void SetUp(TutorialBase ttr){
            _textMesh.Enable = true;
            _catcher.Enabled = true;
            foreach (var gameObject in _highlights){
                ttr.LiftToFront(gameObject);
            }
            _catcher.OnTouched += ttr.StepComplete;
        }

        public void Complete(TutorialBase ttr){
            _textMesh.Enable = false;
            _catcher.Enabled = false;
            foreach (var gameObject in _highlights){
                ttr.PutToBack(gameObject);
            }
            _catcher.OnTouched -= ttr.StepComplete;
        }
    }
}