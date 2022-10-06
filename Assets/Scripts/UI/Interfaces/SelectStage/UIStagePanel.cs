using UnityEngine;
using Model;
using TMPro;
using UnityEngine.UI;

namespace UI.Interfaces.SelectStage
{
    public delegate void ClickEvent(UIStagePanel panel);
    public class UIStagePanel : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Image image;
        public Image highLight;
        private CanvasGroup _group;

        private Stage _model;
        private Transform _parent;

        public Stage Model {
            set {
                _model = value;
                //image.sprite = Resources.Load<Sprite>(value.imgPath);
                text.text = value.desc;
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


