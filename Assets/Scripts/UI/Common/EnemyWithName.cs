using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Loader;

namespace UI.Common
{
    public class EnemyWithName : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public Image enemyImage;

        public void SetEnemy(int id)
        {
            var enemy = CsvLoader.TryToLoad("Configs/enemies", id);
            if (enemy == null)
            {
                GetComponent<Image>().enabled = false;
                nameText.text = "No More";
                enemyImage.enabled = false;
                return;
            }
            GetComponent<Image>().enabled = true;
            enemyImage.enabled = true;
            nameText.text = enemy["desc"] as string;
            enemyImage.sprite = Resources.Load<Sprite>(enemy["img_path"] as string);

        }
        
        public void SetEnemy(Enemy enemy)
        {
            if (enemy == null)
            {
                Debug.Log("disable prefab");
                GetComponent<Image>().enabled = false;
                nameText.text = "No More";
                enemyImage.enabled = false;
                return;
            }
            Debug.Log("enable prefab");
            GetComponent<Image>().enabled = true;
            enemyImage.enabled = true;
            nameText.text = enemy.desc;
            enemyImage.sprite = Resources.Load<Sprite>(enemy.imgPath);
        }
    }

}