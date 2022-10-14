using Core.PlayArea.Balls;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Loader;

namespace UI.GearDisplayer{
    public class GearItem : MonoBehaviour{
        
        public TextMeshProUGUI textMesh;
        public TextMeshProUGUI ballspeed;
        public TextMeshProUGUI ballNum;
        public Image image;
        
        public Image icon;
        public float ballsize;
        
        private Gear _model;
        private Rect _rectTransRect;

        public Gear Model{
            set{
                _model = value;
                UpdateContent();
            }
            get => _model;
        }

        public void UpdateContent()
        {

            var ball = CsvLoader.TryToLoad("Configs/balls", _model.ball.id);
            ballsize = (float)ball["size"];
            RectTransform rectTrans = image.GetComponent<RectTransform>();
            if (ballsize > 1.5)
            {
                ballsize = (float)1.5;
            }
            rectTrans!.localScale = new Vector3(ballsize, ballsize, 1);
            // _rectTransRect = rectTrans.rect;
            // if (_rectTransRect.width > 1.5)
            // {
            //     _rectTransRect.width = (float)1.5;
            // }
            //
            // if (_rectTransRect.height > 1.5)
            // {
            //     _rectTransRect.height = (float)1.5;
            // }
            
            icon.sprite = Resources.Load<Sprite>(_model.imgPath);

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