using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Interfaces.ShopSystem {
    public delegate void ClickEvent(UIShopPanel panel);

    public class UIShopPanel : MonoBehaviour {
        public new TextMeshProUGUI name;
        public TextMeshProUGUI coinWithNumber;
        public TextMeshProUGUI soldOut;
        public TextMeshProUGUI desc;
        public Image image;
        public Image highLight;
        public Button buyButton;
        private CanvasGroup _group;

        private Gear _model;
        private Transform _parent;

        public Gear Model {
            set {
                _model = value;
                image.sprite = Resources.Load<Sprite>(value.imgPath);
                name.text = value.name;
                soldOut.text = "Sold Out";
                soldOut.enabled = false;
                desc.text = value.desc;
            }
            get => _model;
        }

        private int _price;
        public int Price{
            set{
                _price = value;
                coinWithNumber.text = $"{value.ToString()}"; // change to value.price later
                UpdatePriceColor();
            }
            get => _price;
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
        
        public void ShowGearInfo()
        {
            UIManager.shared.OpenUI("UIGearInfo",_model);
        }

        public void UpdatePriceColor(){
            if (Price > GameManager.shared.game.player.Coin){
                coinWithNumber.color = Color.red;
                buyButton.image.color = Color.gray;
            } else{
                coinWithNumber.color = Color.black;
            }
        }
    }
}