using Model;
using TMPro;
using UI.Common;
using UI.Common.SimpleAnimation;


namespace UI.NextEnemyDisplayer{
    public class NextEnemyDisplayer: UIComponent{
        public EdgeHider edgeHider;
        public EnemyWithName shownEnemyItem;
        public TextMeshProUGUI noMoreEnemy;

        private void Start(){
            GameManager.shared.game.currentStage.OnEnemyChanged += UpdateEnemy;
            UIManager.shared.RegisterComponent(this);
            UpdateEnemy(GameManager.shared.game, GameManager.shared.game.currentStage);
        }

        private void UpdateEnemy(Game game, GameModel model){
            var stage = model as Stage;
            if (stage!.NextEnemy != null){
                shownEnemyItem.Enabled = true;
                noMoreEnemy.enabled = false;
                shownEnemyItem.SetEnemy(stage.NextEnemy);
            } else{
                shownEnemyItem.Enabled = false;
                noMoreEnemy.enabled = true;
            }
        }
        

        public override void Hide(){
            edgeHider.Hide();
        }

        public override void Show(){
            edgeHider.Show();
        }
    }
}