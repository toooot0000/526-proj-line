using System;
using Core.PlayArea.Balls;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Loader;

namespace UI.GearDisplayer{
    public class GearItem : MonoBehaviour{

        public Image icon;
        private Gear _model;
        public float ballsize;
        
        public Gear Model{
            set{
                _model = value;
                UpdateContent();
            }
            get => _model;
        }

        public void ShowGearInfo()
        {
            UIManager.shared.OpenUI("UIGearInfo", _model);
        }
        
        public void UpdateContent()
        {
            var ball = CsvLoader.TryToLoad("Configs/balls", Model.ballId);
            ballsize = Mathf.Min((float)ball["size"], 1.5f);
            (transform as RectTransform)!.localScale = new Vector3(ballsize, ballsize, 1);
            icon.sprite = Resources.Load<Sprite>(Model.imgPath);
        }
    }
}