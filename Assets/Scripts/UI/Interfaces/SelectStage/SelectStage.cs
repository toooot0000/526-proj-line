using TMPro;
using UnityEngine;
using Utility.Loader;

namespace UI.Interfaces.SelectStage
{
    public delegate void ClickEvent1(SelectStage panel);
    public class SelectStage : MonoBehaviour
    {
        private CanvasGroup _group;
        
        private Transform _parent;
        public TextMeshProUGUI stagename;

        public int _id;
        
        public int Id
        {
            set
            {
                _id = value;
                var stagetext =  (string)CsvLoader.TryToLoad("Configs/stages", value)["display_name"];
                stagename.text = stagetext;
            }
            get => _id;
        }

        public void GotoStage()
        {
            GameManager.shared.GotoStage(_id);
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

        public event ClickEvent1 OnClick;

        public void Click() {
            OnClick?.Invoke(this);
        }

    }
}

