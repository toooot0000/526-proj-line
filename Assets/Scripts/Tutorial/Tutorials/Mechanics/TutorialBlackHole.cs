using System.Linq;
using Core.PlayArea.BlackHoles;
using Core.PlayArea.Blocks;
using Core.PlayArea.Mines;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.Mechanics{
    public class TutorialBlackHole: TutorialBase{
        public new const string PrefabName = "TutorialBlackHole";
        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;
        private BlackHoleView _target;

        public override void OnLoaded(TutorialManager mng){
            _target = GameManager.shared.playAreaManager.GetAllViews().OfType<BlackHoleView>().First();
            Debug.Assert(_target != null);
            _target.HandOverControlTo(this);
            _target.tutorIsCircleable = false;
            _target.tutorIsSliceable = false;
            var o = _target.gameObject;
            _steps = new StepBase[]{
                // This is a black hole! It will suck balls in.
                new StepTapToContinue<TutorialBlackHole>(texts[0], tapCatcher, o),
                // Circling it can make it shrink!
                new StepTapToContinue<TutorialBlackHole>(texts[1], tapCatcher, o)
            };
            base.OnLoaded(mng);
        }

        protected override void Complete() {
            _target.GainBackControlFrom(this);
            base.Complete();
        }
    }
}