using UnityEngine;
using Model;
using TMPro;
using UnityEngine.UI;
using Utility.Loader;

namespace UI.Interfaces.SelectStage
{
    public delegate void ClickEvent(UIStagePanel panel);
    public class UIStagePanel : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public TextMeshProUGUI geartype;
        public TextMeshProUGUI bonusCoins;
        public Image image;
        public Image highLight;
        private CanvasGroup _group;

        private Stage _model;
        private Transform _parent;

        public int _id;
        public int Id{
            set{
                _id = value;
                text.text = (string)CsvLoader.TryToLoad("Configs/stages", value)["desc"];
                geartype.text = "stage type: " + (string)CsvLoader.TryToLoad("Configs/stages", value)["type"];
                bonusCoins.text = (string)CsvLoader.TryToLoad("Configs/stages", value)["desc"] + " clear" +
                                  " bonus coins: " + CsvLoader.TryToLoad("Configs/stages", value)["bonus_coins"] as string;
                
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


