using System;
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
            var ball = CsvLoader.TryToLoad("Configs/balls", Model.ballId);
            ballsize = Mathf.Min((float)ball["size"], 1.5f);
            (transform as RectTransform)!.localScale = new Vector3(ballsize, ballsize, 1);
            icon.sprite = Resources.Load<Sprite>(Model.imgPath);
            textMesh.text = Model.ToDescString();
            ballspeed.text = (float)ball["speed"] switch {
                <=0 => "",
                >0 and <=0.5f => ">",
                >0.5f and <=1.5f => ">>",
                _ => ">>>"
            };
            ballNum.text = $"x {Model.ballNum.ToString()}";
        }
    }
}