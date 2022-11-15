using System.Linq;
using Core.PlayArea.Blocks;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.Mechanics{
    public class TutorialBlock: TutorialBase{
        public new const string PrefabName = "TutorialBlock";
        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;

        public override void OnLoaded(TutorialManager mng){
            var block = GameManager.shared.playAreaManager.GetAllViewsOfType<BlockView>().First();
            Debug.Assert(block != null);
            _steps = new StepBase[]{
                // This is block! Balls that collide into it will be bouncing back.
                new StepTapToContinue<TutorialBlock>(texts[0], tapCatcher, block.gameObject),
                // And also you can't draw lines over it.
            };
            
            base.OnLoaded(mng);
        }

        protected override void Complete() {
            
            base.Complete();
        }
    }
}