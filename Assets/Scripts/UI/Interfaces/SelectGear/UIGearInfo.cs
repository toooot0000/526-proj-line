using Model;
using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Loader;

namespace UI.Interfaces.SelectGear
{
    public class UIGearInfo : UIBase
    {
        
        public TextMeshProUGUI gearName;
        public TextMeshProUGUI gearDesc;
        public TextMeshProUGUI details;
        private CanvasGroup _canvasGroup;
        public Image icon;

        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public override void Open(object model)
        {
            var gearModel = (Gear)model;
            base.Open(model);
            gearName.text = gearModel.name;
            gearDesc.text =  gearModel.desc;
            icon.sprite = Resources.Load<Sprite>(gearModel.imgPath);
            details.text = gearModel.ToDescString();
            StartCoroutine(FadeIn(_canvasGroup));
        }

        public override void Close()
        {
            StartCoroutine(FadeOut(_canvasGroup, () => {
                base.Close();
                Destroy(gameObject);
            }));
        }

        public void TapToContinue()
        {
            Close();
        }

    }
}