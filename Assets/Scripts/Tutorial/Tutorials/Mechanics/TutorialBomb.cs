using System.Linq;
using Core.PlayArea.Blocks;
using Core.PlayArea.Mines;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.Mechanics{
    public class TutorialBomb: TutorialBase{
        public new const string PrefabName = "TutorialBomb";
        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;
        private MineView _target;

        public override void OnLoaded(TutorialManager mng){
            _target = GameManager.shared.playAreaManager.GetAllViews().OfType<MineView>().First();
            Debug.Assert(_target != null);
            _target.HandOverControlTo(this);
            _target.tutorIsCircleable = false;
            _target.tutorIsSliceable = false;
            var o = _target.gameObject;
            _steps = new StepBase[]{
                // This is a bomb. Don't touch it. It will explode if you touch it!
                new StepTapToContinue<TutorialBomb>(texts[0], tapCatcher, o),
                // Circling it can remove it from the game!
                new StepTapToContinue<TutorialBomb>(texts[1], tapCatcher, o)
            };
            base.OnLoaded(mng);
        }

        protected override void Complete() {
            _target.GainBackControlFrom(this);
            base.Complete();
        }
    }
}