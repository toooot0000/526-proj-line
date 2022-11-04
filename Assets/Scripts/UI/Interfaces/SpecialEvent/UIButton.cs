using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

namespace UI.Interfaces.SpecialEvent

{
    public delegate void ClickEvent(UIButton button);
    
    public class UIButton: MonoBehaviour
    {
        
        public TextMeshProUGUI text;
        public Image image;
        private CanvasGroup _group;
        private EventChoice _model;
        private Transform _parent;
        

        public EventChoice Model{
            set{
                _model = value;
                text.text = value.desc;
            }
            get => _model;
        }

        public bool Show{
            set{
                gameObject.SetActive(value);
                if (value){
                    transform.SetParent(_parent);
                } else{
                    _parent = transform.parent;
                    transform.SetParent(null);
                }
            }
        }
        
        public bool Interactable{
            set
            {
                Button _btn = gameObject.GetComponent<Button>();
                _btn.interactable = value;
                _btn.image.color = Color.gray;

            }
        }
        

        private void Start(){
            _group = GetComponent<CanvasGroup>();
            _parent = transform.parent;
        }

        public event ClickEvent OnClick;

        public void Click(){
            OnClick?.Invoke(this);
        }
        
    }
}