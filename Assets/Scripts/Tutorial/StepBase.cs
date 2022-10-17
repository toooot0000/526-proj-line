using System;

namespace Tutorial{
    public delegate void StepCallbackDelegate(TutorialBase tutorial, StepBase step);
    
    public abstract class StepBase{
        public abstract void SetUp(TutorialBase tutorial);
        public abstract void Complete(TutorialBase tutorial);
    }
}