using System;

namespace Tutorials{
    public interface IStepBase{
        void SetUp(TutorialBase tutorial);
        void Complete(TutorialBase tutorial);
    }
}