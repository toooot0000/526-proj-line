using System;

namespace Tutorials{
    public abstract class StepBase<T> where T : TutorialBase{
        public abstract void SetUp(T tutorial);
        public abstract void Complete(T tutorial);
    }
}