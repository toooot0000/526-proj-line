using System;

namespace Tutorials{
    public abstract class StepBase{
        public abstract void SetUp(TutorialBase tutorial);
        public abstract void Complete(TutorialBase tutorial);
    }
}