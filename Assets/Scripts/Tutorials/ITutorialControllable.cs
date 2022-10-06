namespace Tutorials{
    public delegate void TutorialControllableEvent(ITutorialControllable controllable);

    public interface ITutorialControllable{
        public void HandOverControlTo(TutorialBase tutorial);
        public void GainBackControlFrom(TutorialBase tutorial);
    }
}