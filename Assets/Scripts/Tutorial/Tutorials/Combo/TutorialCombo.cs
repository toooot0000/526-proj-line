namespace Tutorial.Tutorials.Combo{
    public class TutorialCombo: TutorialBase{
        private StepBase[] _step;
        protected override StepBase[] Steps => _step;

        public override void OnLoaded(TutorialManager mng){
            
            base.OnLoaded(mng);
        }
    }
}