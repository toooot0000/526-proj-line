namespace Tutorials.Charge{
    /// <summary>
    /// 1. "Enemy is going to do a special attack. Better to be prepared!"
    /// 2. "Circle the shield ball to trigger the charge effect"
    /// 3. "You can check your current action result here.\nNow you will gain 5 shield that will counter the enemy's heavy attack!"
    /// </summary>
    public partial class TutorialCharge: TutorialBase{
        protected override StepBase[] Steps{ get; } = new StepBase[]{
            new Step1(), new Step2(), new Step3()
        };

        public override void Load(TutorialManager mng){
            base.Load(mng);
            
        }

        private partial class Step1: StepBase{ }
        private partial class Step2: StepBase{ }
        private partial class Step3: StepBase{ }
    }
}