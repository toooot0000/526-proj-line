using System;
using Model.Buff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Interfaces.BuffDetail{

    public class UIBuffDetailOption : IUISetUpOptions<UIBuffDetail>{
        public Buff buff;
        public void ApplyOptions(UIBuffDetail ui){
            ui.icon.sprite = buff.IconSprite;
            ui.detailText.text = buff.ToDetailString();
        }
    }

    public class UIBuffDetail: UIBase{
        public Image icon;
        public TextMeshProUGUI detailText;
        public CanvasGroup canvasGroup;

        private void Start(){
            canvasGroup.alpha = 0;
        }

        public override void Open(object arg){
            base.Open(arg);
            StartCoroutine(UIBase.FadeIn(canvasGroup));
        }

        public override void Close(){
            StartCoroutine(FadeOut(canvasGroup, () => {
                base.Close();
            }));
        }
    }
}