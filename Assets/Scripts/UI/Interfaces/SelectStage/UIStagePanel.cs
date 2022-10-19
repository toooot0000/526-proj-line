using System;
using System.Linq;
using UnityEngine;
using Model;
using TMPro;
using UnityEngine.UI;
using Utility;
using Utility.Loader;

namespace UI.Interfaces.SelectStage
{
    public delegate void ClickEvent(UIStagePanel panel);
    public class UIStagePanel : MonoBehaviour
    {
       
        public TextMeshProUGUI text;
        public TextMeshProUGUI stagetype;

        public Image image;
        public Image highLight;
        public Image enemyImage;
        public Image enemyImage1;
        public Image enemyImage2;
        public Image bonusImage;
        public Image bonusImage1;
        public TextMeshProUGUI bonusText;
        public TextMeshProUGUI enemyName;
        public TextMeshProUGUI enemyName1;
        public TextMeshProUGUI enemyName2;
        
        private CanvasGroup _group;

        private Stage _model;
        private Transform _parent;

        public int _id;
        public int[] nextEnemyIds;
        public int[] bonusGearIds;
        public int coin;

        public String enemy;
        public String enemy1;
        public String enemy2;
        public String gear;
        public String gear1;
        public String enemyname;
        public String enemyname1;
        public String enemyname2;
        public String next_stage_choice;
        
        public int Id
        {
            set
            {
                _id = value;

                var bonusGearsStr = (string)CsvLoader.TryToLoad("Configs/stages", value)["bonus_gears"];
                if (!string.IsNullOrEmpty(bonusGearsStr))
                {
                    bonusGearIds = bonusGearsStr.Split(";").Select(IntUtility.ParseString).ToArray();
                }

                text.text = (string)CsvLoader.TryToLoad("Configs/stages", value)["desc"];
                stagetype.text = (string)CsvLoader.TryToLoad("Configs/stages", value)["type"];
                next_stage_choice = (string)CsvLoader.TryToLoad("Configs/stages", value)["next_stage_choices"];
                
                var nextEnemyStr = (string)CsvLoader.TryToLoad("Configs/stages", value)["enemies"];
                if (!string.IsNullOrEmpty(nextEnemyStr))
                {
                    nextEnemyIds = nextEnemyStr.Split(";").Select(IntUtility.ParseString).ToArray();
                }

                if (next_stage_choice == "-1")
                {
                    if (nextEnemyIds.Length == 1)
                    {
                        enemyname = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["name"];
                        enemyName.text = enemyname;
                        enemyName.enabled = true;
                        enemy = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["img_path"];
                        enemyImage.sprite = Resources.Load<Sprite>(enemy);
                        
                        enemyName1.enabled = false;
                        enemyImage1.enabled = false;
                        enemyName2.enabled = false;
                        enemyImage2.enabled = false;
                        bonusText.text = "Last Stage";
                        bonusImage.sprite = Resources.Load<Sprite>("Images/Common/lastlevel");
                        bonusImage1.enabled = false;
                    }
                    
                    else if (nextEnemyIds.Length == 2)
                    {
                        enemyname = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["name"];
                        enemy = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["img_path"];
                        enemyName.text = enemyname;
                        enemyName.enabled = true;
                        enemyImage.sprite = Resources.Load<Sprite>(enemy);
                        enemyname1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["name"];
                        enemy1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["img_path"];
                        enemyName1.text = enemyname1;
                        enemyName1.enabled = true;
                        enemyImage1.sprite = Resources.Load<Sprite>(enemy1);
                    
                        enemyName2.gameObject.SetActive(false);
                        enemyImage2.enabled = false;
                        bonusText.text = "Last Stage";
                        bonusImage.sprite = Resources.Load<Sprite>("Images/Common/lastlevel");
                        bonusImage1.enabled = false;
                    }
                        
                    else if (nextEnemyIds.Length == 3)
                    {
                        enemyname = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["name"];
                        enemy = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["img_path"];
                        enemyName.text = enemyname;
                        enemyName.enabled = true;
                        enemyImage.sprite = Resources.Load<Sprite>(enemy);
                            
                        enemyname1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["name"];
                        enemy1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["img_path"];
                        enemyName1.text = enemyname1;
                        enemyName1.enabled = true;
                        enemyImage1.sprite = Resources.Load<Sprite>(enemy1);
                            
                        enemyname2 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[2])["name"];
                        enemy2 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[2])["img_path"];
                        enemyName2.text = enemyname2;
                        enemyName2.enabled = true;
                        enemyImage2.sprite = Resources.Load<Sprite>(enemy2);
                            
                        bonusText.text = "Last Stage";
                        bonusImage.sprite = Resources.Load<Sprite>("Images/Common/lastlevel");
                        bonusImage1.enabled = false;
                    }
                    else
                    {
                        enemyName.gameObject.SetActive(false);
                        enemyName1.gameObject.SetActive(false);
                        enemyName2.gameObject.SetActive(false);
                        enemyImage.enabled = false;
                        enemyImage1.enabled = false;
                        enemyImage2.enabled = false;
                    }
                    
                }
                else
                {
                    switch (stagetype.text)
                    {
                        case "shop":
                            
                            enemyName.gameObject.SetActive(false);
                            enemyName1.gameObject.SetActive(false);
                            enemyName2.gameObject.SetActive(false);
                            enemyImage.enabled = false;
                            enemyImage1.enabled = false;
                            enemyImage2.enabled = false;
                            
                            bonusText.text = "StageType: shop";
                            bonusImage.sprite = Resources.Load<Sprite>("Images/Common/shop");
                            bonusImage1.enabled = false;
                            break;
                        case "event":
                            
                            enemyName.gameObject.SetActive(false);
                            enemyName1.gameObject.SetActive(false);
                            enemyName2.gameObject.SetActive(false);
                            enemyImage.enabled = false;
                            enemyImage1.enabled = false;
                            enemyImage2.enabled = false;
                            
                            bonusText.text = "StageType: event";
                            bonusImage.sprite = Resources.Load<Sprite>("Images/Common/event");
                            bonusImage1.enabled = false;
                            break;
                        
                        case "battle":
                        {
                            if (nextEnemyIds.Length == 1)
                            {
                               enemyname = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["name"];
                               enemy = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["img_path"];
                               enemyName.text = enemyname;
                               enemyName.enabled = true;
                               enemyImage.sprite = Resources.Load<Sprite>(enemy);

                               enemyName1.gameObject.SetActive(false);
                               enemyImage1.enabled = false;
                               enemyName2.gameObject.SetActive(false);
                               enemyImage2.enabled = false;
                            }

                            else if (nextEnemyIds.Length == 2)
                            {  
                                enemyname = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["name"];
                                enemy = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["img_path"];
                                enemyName.text = enemyname;
                                enemyName.enabled = true;
                                enemyImage.sprite = Resources.Load<Sprite>(enemy);
                                enemyname1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["name"];
                                enemy1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["img_path"];
                                enemyName1.text = enemyname1;
                                enemyName1.enabled = true;
                                enemyImage1.sprite = Resources.Load<Sprite>(enemy1);

                                enemyName2.gameObject.SetActive(false);
                                enemyImage2.enabled = false;
                                
                            }
                        
                            else if (nextEnemyIds.Length == 3)
                            {
                                enemyname = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["name"];
                                enemy = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[0])["img_path"];
                                enemyName.text = enemyname;
                                enemyName.enabled = true;
                                enemyImage.sprite = Resources.Load<Sprite>(enemy);
                            
                                enemyname1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["name"];
                                enemy1 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[1])["img_path"];
                                enemyName1.text = enemyname1;
                                enemyName1.enabled = true;
                                enemyImage1.sprite = Resources.Load<Sprite>(enemy1);

                                enemyname2 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[2])["name"];
                                enemy2 = (string)CsvLoader.TryToLoad("Configs/enemies", nextEnemyIds[2])["img_path"];
                                enemyName2.text = enemyname2;
                                enemyName2.enabled = true;
                                enemyImage2.sprite = Resources.Load<Sprite>(enemy2);
                            }

                            else
                            {
                                enemyName.gameObject.SetActive(false);
                                enemyName1.gameObject.SetActive(false);
                                enemyName2.gameObject.SetActive(false);
                                enemyImage.enabled = false;
                                enemyImage1.enabled = false;
                                enemyImage2.enabled = false;
                            }
                            
                            coin = (int)CsvLoader.TryToLoad("Configs/stages", value)["bonus_coins"];

                            if (coin != -1)
                            {
                                bonusText.text = "Bonus Coins:" + $"{coin.ToString()} x ";
                                bonusImage.sprite = Resources.Load<Sprite>("Images/Common/coin");
                                bonusImage1.enabled = false;
                            }

                            if (coin == -1 && next_stage_choice != "-1")
                            {
                                bonusText.text = "Bonus Gears:";
                                
                                if (bonusGearIds.Length == 1)
                                {
                                    gear = (string)CsvLoader.TryToLoad("Configs/gears", bonusGearIds[0])["img_path"];
                                    bonusImage.sprite = Resources.Load<Sprite>(gear);
                                    bonusImage1.enabled = false;
                                }
                                else if (bonusGearIds.Length == 2)
                                {
                                    gear = (string)CsvLoader.TryToLoad("Configs/gears", bonusGearIds[0])["img_path"];
                                    bonusImage.sprite = Resources.Load<Sprite>(gear);
                                    gear1 = (string)CsvLoader.TryToLoad("Configs/gears", bonusGearIds[1])["img_path"];
                                    bonusImage1.sprite = Resources.Load<Sprite>(gear1);
                                }
                            }

                            break;
                        }
                    }
                }
            }
            get => _id;
        }
    
        
        public bool Show {
            set {
                gameObject.SetActive(value);
                if (value) {
                    transform.SetParent(_parent);
                }
                else {
                    _parent = transform.parent;
                    transform.SetParent(null);
                }
            }
        }

        private void Start() {
            _group = GetComponent<CanvasGroup>();
            _parent = transform.parent;
        }

        public event ClickEvent OnClick;

        public void Click() {
            OnClick?.Invoke(this);
        }
        
    }
}


