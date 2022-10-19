using Model;
using UI.Common;
using UI.Common.SimpleAnimation;


namespace UI.NextEnemyDisplayer{
    public class NextEnemyDisplayer: UIComponent{
        public EdgeHider edgeHider;
        //public Image enemyImage;
        //public TextMeshProUGUI nameText;
        //public EnemyItem enemyItemPrefab;
        public EnemyWithName enemyWithNamePrefab;
        private EnemyWithName _shownEnemyItem;

        private void Start(){
            GameManager.shared.game.currentStage.OnEnemyChanged += UpdateEnemy;
            UIManager.shared.RegisterComponent(this);
            //UpdateEnemy(GameManager.shared.game, GameManager.shared.game.currentStage);
        }

        private void UpdateEnemy(Game game, GameModel stage)
        {
            if(_shownEnemyItem != null) Destroy(_shownEnemyItem.gameObject);
            _shownEnemyItem = Instantiate(enemyWithNamePrefab, transform);
            _shownEnemyItem.SetEnemy((stage as Stage)?.NextEnemy);
        }
        

        public override void Hide(){
            edgeHider.Hide();
        }

        public override void Show(){
            edgeHider.Show();
        }
    }
}