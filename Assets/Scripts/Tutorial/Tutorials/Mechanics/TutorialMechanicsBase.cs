using System.Linq;
using Core.PlayArea;
using Core.PlayArea.Blocks;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.Mechanics{
    public class TutorialMechanicsBase<T>: TutorialBase
    where T: PlayableObjectViewBase{
        public new const string PrefabName = "TutorialBlock";
        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;
        private T _target;

        public override void OnLoaded(TutorialManager mng){
            _target = GameManager.shared.playAreaManager.GetAllViews().OfType<T>().First();
            _target.HandOverControlTo(this);
            Debug.Assert(_target != null);
            var o = _target.gameObject;
            _steps = new StepBase[]{
                // This is block! Balls that collide into it will be bouncing back.
                new StepTapToContinue<TutorialBlock>(texts[0], tapCatcher, o),
                // And also you can't draw lines over it.
                new StepTapToContinue<TutorialBlock>(texts[1], tapCatcher, o)
            };
            base.OnLoaded(mng);
        }

        protected override void Complete() {
            _target.GainBackControlFrom(this);
            base.Complete();
        }
    }
}