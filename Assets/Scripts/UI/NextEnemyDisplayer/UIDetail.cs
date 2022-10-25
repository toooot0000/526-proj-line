using System;
using Model;
using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Event = UnityEngine.Event;

namespace UI.NextEnemyDisplayer
{
    public class UIDetail:UIBase
    {
        public Image enemyImage;
        public TextMeshProUGUI name;
        public TextMeshProUGUI desc;
        public TextMeshProUGUI hp;
        public TextMeshProUGUI atk;
        private CanvasGroup _canvasGroup;
        public Enemy enemyModel;
        private bool _inAnimation;
        
        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public override void Open(object model)
        {
            enemyModel = (Enemy)model;
            base.Open(model);
            _inAnimation = true;

            enemyImage.sprite = Resources.Load<Sprite>(enemyModel.imgPath);
            name.text = enemyModel.name;
            desc.text = enemyModel.desc;
            hp.text = "HP: " + enemyModel.HpUpLimit;
            atk.text = "ATK: " + enemyModel.attack;
            
            
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