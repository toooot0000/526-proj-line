using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearDisplayer{
    public class GearItem : MonoBehaviour{
        public Image image;
        public TextMeshProUGUI textMesh;

        private Gear _model;

        public Gear Model{
            set{
                _model = value;
                UpdateContent();
            }
            get => _model;
        }

        public void UpdateContent(){
            image.sprite = Resources.Load<Sprite>(_model.imgPath);
            textMesh.text = Model.ToDescString();
        }
    }
}