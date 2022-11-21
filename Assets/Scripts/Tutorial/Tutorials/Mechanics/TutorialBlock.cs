using System.Linq;
using Core.PlayArea.Blocks;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.Mechanics{
    public class TutorialBlock : TutorialBase{
        public new const string PrefabName = "TutorialBlock";
        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;
        private BlockView _target;

        public override void OnLoaded(TutorialManager mng){
            _target = GameManager.shared.playAreaManager.GetAllViews().OfType<BlockView>().First();
            _target.HandOverControlTo(this);
            Debug.Assert(_target != null);
            var o = _target.gameObject;
            _steps = new StepBase[]{
                // This is block! Balls that collide into it will be bouncing back.
                new StepTapToContinue<TutorialBlock>(texts[0], tapCatcher, o),
                // And also you can't draw lines over it.
                new StepTapToContinue<TutorialBlock>(texts[1], tapCatcher, o),
                // The number on it shows how many turns past it will be removed.
                new StepTapToContinue<TutorialBlock>(texts[2], tapCatcher, o)
            };
            base.OnLoaded(mng);
        }

        protected override void Complete() {
            _target.GainBackControlFrom(this);
            base.Complete();
        }
    }
}