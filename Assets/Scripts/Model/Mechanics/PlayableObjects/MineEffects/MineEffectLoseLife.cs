namespace Model.Mechanics.PlayableObjects.MineEffects{
    public class MineEffectLoseLife: MineEffect{
        public readonly int point;

        public MineEffectLoseLife(int point = 1) {
            this.point = point;
        }
        
        public override void Execute() {
            var game = GameManager.shared.game;
            new Damage(game, Damage.Type.Physics, point, game.player).Resolve();
        }
    }
}