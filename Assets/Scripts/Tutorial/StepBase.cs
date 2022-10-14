using Tutorials;

namespace Tutorial{
    public interface IStepBase{
        void SetUp(TutorialBase tutorial);
        void Complete(TutorialBase tutorial);
    }
}