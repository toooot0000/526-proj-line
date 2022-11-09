using Core.PlayArea;

namespace Model.Mechanics.PlayableObjects.CrystalEffects {
    public class CrystalEffectChangeMovableSpeed: ICrystalEffect {
        public float factor = 0.5f;
        public void Execute() {
            var allMovables = 
                GameManager.shared.playAreaManager.GetAllViewsOfProperty<IMovableView>();
            foreach (var movable in allMovables) {
                movable.VelocityMultiplier *= factor;
            }
        }

        public void Reset() {
            var allMovables = 
                GameManager.shared.playAreaManager.GetAllViewsOfProperty<IMovableView>();
            foreach (var movable in allMovables) {
                movable.VelocityMultiplier /= factor;
            }
        }
    }
}