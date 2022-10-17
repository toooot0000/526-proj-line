using System;

namespace Tutorial{
    
    public abstract class StepBase{
        public delegate void BindEventDelegate(StepBase stepBase);
        public abstract void SetUp(TutorialBase tutorial);
        public abstract void Complete(TutorialBase tutorial);
        public virtual Action<TutorialBase, StepBase> BindEvent{ get; protected set; } = null;
    }
}