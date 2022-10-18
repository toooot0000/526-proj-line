using Tutorial.Common;
using Tutorial.UI;

namespace Tutorial.Tutorials.MultipleEnemy{
    public class TutorialMultipleEnemy: UITutorialBase{

        public TutorialText[] texts;
        
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;

        public override void OnLoaded(TutorialManager mng){
            
                        
            
            _steps = new StepBase[]{ };
            
            
            base.OnLoaded(mng);
        }
    }
}