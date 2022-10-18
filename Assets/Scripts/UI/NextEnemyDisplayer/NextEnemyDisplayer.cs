using Model;
using TMPro;
using UI.Common.SimpleAnimation;
using UnityEngine;
using UnityEngine.UI;

namespace UI.NextEnemyDisplayer{
    public class NextEnemyDisplayer: UIComponent{
        public EdgeHider edgeHider;
        public Image enemyImage;
        public TextMeshProUGUI nameText;
        public EnemyItem enemyItemPrefab;
        private EnemyItem shownEnemyItem;

        private void Start(){
            GameManager.shared.game.currentStage.OnEnemyChanged += UpdateEnemy;
            UIManager.shared.RegisterComponent(this);
            //UpdateEnemy(GameManager.shared.game, GameManager.shared.game.currentStage);
        }

        private void UpdateEnemy(Game game, GameModel stage)
        {
            if(shownEnemyItem != null) Destroy(shownEnemyItem.gameObject);
            shownEnemyItem = Instantiate(enemyItemPrefab, transform);
            shownEnemyItem.UpdateContent();
        }
        

        public override void Hide(){
            edgeHider.Hide();
        }

        public override void Show(){
            edgeHider.Show();
        }
    }
}