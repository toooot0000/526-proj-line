namespace Model.Obstacles.MineEffects{
    public class MineEffectLoseLife: MineEffect{
        public int point = 1;
        public override void Execute(Player player){
            player.CurrentHp -= point;
        }
    }
}