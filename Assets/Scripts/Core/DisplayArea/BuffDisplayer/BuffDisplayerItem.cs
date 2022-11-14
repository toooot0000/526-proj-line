using Model;
using Model.Buff;
using TMPro;
using UI;
using UI.Interfaces.BuffDetail;
using UnityEngine;
using UnityEngine.UI;

namespace Core.DisplayArea.BuffDisplayer{
    public class BuffDisplayerItem: MonoBehaviour{

        public Image img;
        public TextMeshProUGUI text;

        private Buff _buff;
        public Buff Model{
            set{
                if (_buff != null){
                    _buff.OnBuffLayerAdded -= UpdateLayerText;
                    _buff.OnBuffLayerRemoved -= UpdateLayerText;
                }
                _buff = value;
                img.sprite = value.IconSprite;
                text.text = $"{value.layer.ToString()}";
                _buff.OnBuffLayerAdded += UpdateLayerText;
                _buff.OnBuffLayerRemoved += UpdateLayerText;
            }
            get => _buff;
        }

        public void OnClick(){
            UIManager.shared.OpenUI("UIBuffDetail", new UIBuffDetailOption(){
                buff = Model
            });
        }

        private void UpdateLayerText(Game game, Buff buff){
            text.text = $"{buff.layer.ToString()}";
        }
    }
}