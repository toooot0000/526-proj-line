using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Interfaces.ShopSystem {
    public delegate void ClickEvent(UIShopPanel panel);

    public class UIShopPanel : MonoBehaviour {
        public TextMeshProUGUI name;
        public TextMeshProUGUI price;
        public TextMeshProUGUI desc;
        public Image image;
        public Image highLight;
        private CanvasGroup _group;

        private Gear _model;
        private Transform _parent;

        public Gear Model {
            set {
                _model = value;
                image.sprite = Resources.Load<Sprite>(value.imgPath);
                name.text = value.name;
                price.text = value.rarity.ToString(); // change to value.price later
                desc.text = value.desc;
            }
            get => _model;
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