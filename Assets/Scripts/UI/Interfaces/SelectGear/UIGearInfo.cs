using System;
using System.Collections.Generic;
using Model;
using Model.GearEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Loader;

namespace UI.Interfaces.SelectGear
{
    public class UIGearInfo:UIBase
    {
        public Image _gearImage;
        public TextMeshProUGUI _gearName;
        public TextMeshProUGUI _gearDesc;
        private CanvasGroup _canvasGroup;
        public Gear gearModel;
        private bool _inAnimation;
        public Image bg;
        public Image image;
        public Image icon;
        public TextMeshProUGUI desc;
        public TextMeshProUGUI descInfo;
        public float ballsize;
        public float ballspeed;
        public float ballpoint;
        public TextMeshProUGUI ballsizetext;
        public TextMeshProUGUI ballspeedtext;
        public GearType type;

        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public override void Open(object model)
        { 
            gearModel = (Gear)model;
            base.Open(model);
            _inAnimation = true;
            _gearImage.sprite = Resources.Load<Sprite>(gearModel.imgPath);
            _gearName.text = gearModel.name;
            _gearDesc.text =  gearModel.desc;
            var ball = CsvLoader.TryToLoad("Configs/balls", gearModel.ballId);
            ballsize = (float)ball["size"];
            ballspeed = (float)ball["speed"];
            ballpoint = (int)ball["point"];

            RectTransform rectTrans = image.GetComponent<RectTransform>();
            rectTrans!.localScale = new Vector3(ballsize, ballsize, 1);
            icon.sprite = Resources.Load<Sprite>(gearModel.imgPath);
            ballsizetext.text = "Ball size: " + ballsize as string;
            ballspeedtext.text = "Ball speed: " + ballspeed as string;
            desc.text = gearModel.ToDesc();
            descInfo.text = gearModel.ToDescComboCharge();

            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            StartCoroutine(coroutine());
        }
        
        public override void Close() {
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 1,
                i => _canvasGroup.alpha = 1 - i,
                () => {
                    _inAnimation = false;
                    base.Close();
                    Destroy(gameObject);
                });
            StartCoroutine(coroutine());
        } 
        
        public void TapToContinue()
        {
            Close();
        }
    }

}

