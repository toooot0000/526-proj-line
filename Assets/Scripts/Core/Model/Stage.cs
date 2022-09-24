using UI;

namespace Core.Model{
    public class Stage: GameModel{

        public Enemy[] enemies;
        public int enemies_solved;

        public event ModelEvent OnStageBeaten;

        public Stage(GameModel parent, Enemy[] enemies) : base(parent)
        {
            this.enemies = enemies;
            this.enemies_solved = 0;
            enemies[enemies.Length-1].OnDie +=(game, model) =>
                {
                    this.OnStageBeaten?.Invoke(currentGame,this);
                };
        }

        private void OpenResolveWindow()
        {
           UIBase resolveWindow = UIManager.shared.OpenUI("UISelectGear");
           resolveWindow.Open();
           UISelectGear selectGear = resolveWindow.GetComponent<UISelectGear>();
           GearController gearController = selectGear.panel.GetComponent<GearController>();
           Gear gear = new Gear(GameManager.shared.game);
           // gearController.UpdateContent(gear); //暂时改第一个装备

        }
    }
}