using Core.PlayArea.Blocks;
using UnityEngine;

namespace Tutorial.Tutorials.Mechanics{
    public class TutorialBlock: TutorialBase{
        public new const string PrefabName = "TutorialBlock";
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;

        public override void OnLoaded(TutorialManager mng){
            var block = GameManager.shared.playAreaManager.GetAllViewsOfType<BlockView>();
            Debug.Assert(block != null);
            _steps = new StepBase[]{
                
            };
            
            

            base.OnLoaded(mng);
        }
    }
}