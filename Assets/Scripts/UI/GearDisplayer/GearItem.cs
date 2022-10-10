using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearDisplayer{
    public class GearItem : MonoBehaviour{
        public Image image;
        public TextMeshProUGUI textMesh;
        public TextMeshProUGUI ballspeed;
        public TextMeshProUGUI ballNum;

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
            
            if (_model.ball.speed <= 0)
            {
                ballspeed.text = "";
            }
            if (_model.ball.speed > 0 && _model.ball.speed <= 0.5)
            {
                ballspeed.text = ">";
            }
            if ( _model.ball.speed > 0.5 && _model.ball.speed <= 1.5)
            {
                ballspeed.text = ">>";
            }
            else
            {
                ballspeed.text = ">>>";
            }
            
            ballNum.text = $"x {_model.ballNum.ToString()}";
        }
    }
}