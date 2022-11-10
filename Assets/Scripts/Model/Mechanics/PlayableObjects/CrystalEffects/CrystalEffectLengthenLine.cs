namespace Model.Mechanics.PlayableObjects.CrystalEffects {
    public class CrystalEffectLengthenLine: CrystalEffect{
        public float value = 3f;
        private bool _isExecuted = false;
        public override void Execute(){
            if (_isExecuted) return;
            GameManager.shared.touchTracker.lineLengthAdder += value;
            GameManager.shared.playAreaManager.RegisterResetEffect(this);
            _isExecuted = true;
        }

        public override void Reset(){
            GameManager.shared.touchTracker.lineLengthAdder -= value;
            _isExecuted = false;
        }
    }
}