namespace Tutorials{

    public delegate void TutorialControllableEvent(ITutorialControllable controllable);
    public interface ITutorialControllable{
        public void ControlledByTutorial(TutorialBase tutorial);
        public void GainBackControl(TutorialBase tutorial);
    }
}