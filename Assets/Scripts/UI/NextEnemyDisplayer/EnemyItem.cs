using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.NextEnemyDisplayer
{
    public class EnemyItem: MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public Image enemyImage;
        
        public void UpdateContent()
        {
            Enemy nextEnemy = GameManager.shared.game.currentStage.NextEnemy;
            if(nextEnemy == null)
            {
                GetComponent<Image>().enabled = false;
                nameText.text = "No More";
                enemyImage.enabled = false;
                return;
            }
            GetComponent<Image>().enabled = true;
            enemyImage.enabled = true;
            nameText.text = nextEnemy.desc;
            enemyImage.sprite = Resources.Load<Sprite>(nextEnemy.imgPath);
        }
    }
    
}