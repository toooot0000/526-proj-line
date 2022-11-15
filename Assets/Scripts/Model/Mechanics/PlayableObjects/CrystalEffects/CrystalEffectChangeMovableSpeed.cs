using Core.PlayArea;

namespace Model.Mechanics.PlayableObjects.CrystalEffects {
    public class CrystalEffectChangeMovableSpeed: CrystalEffect {
        public float factor = 0.5f;
        private bool _isExecuted = false;
        public override void Execute() {
            if (_isExecuted) return;
            var allMovables = 
                GameManager.shared.playAreaManager.GetAllViewsOfProperty<IMovableView>();
            foreach (var movable in allMovables) {
                movable.VelocityMultiplier *= factor;
            }
            GameManager.shared.playAreaManager.RegisterResetEffect(this);
            _isExecuted = true;
        }

        public override void Reset() {
            var allMovables = 
                GameManager.shared.playAreaManager.GetAllViewsOfProperty<IMovableView>();
            foreach (var movable in allMovables) {
                movable.VelocityMultiplier /= factor;
            }
            _isExecuted = false;
        }
    }
}