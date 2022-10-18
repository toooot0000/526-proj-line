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
        public Image _img;

        private void Start(){
            GameManager.shared.game.currentStage.OnEnemyChanged += UpdateEnemy;
            UIManager.shared.RegisterComponent(this);
            UpdateEnemy(GameManager.shared.game, GameManager.shared.game.currentStage);
        }

        private void UpdateEnemy(Game game, GameModel stage)
        {
            Enemy nextEnemy = (stage as Stage)!.NextEnemy;
            if(nextEnemy == null)
            {
                //GameObject.GetComponent<Image>().enabled = false;
                GameObject.Find("Image").SetActive(false);
                nameText.text = "No More";
                enemyImage.sprite = null;
                return;
            }
            nameText.text = nextEnemy.desc;
            enemyImage.sprite = Resources.Load<Sprite>(nextEnemy.imgPath);
        }
        

        public override void Hide(){
            edgeHider.Hide();
        }

        public override void Show(){
            edgeHider.Show();
        }
    }
}