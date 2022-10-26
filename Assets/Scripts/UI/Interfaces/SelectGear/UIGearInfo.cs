using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Interfaces.SelectGear
{
    public class UIGearInfo:UIBase
    {
        public Image _gearImage;
        public TextMeshProUGUI _gearDesc;
        public TextMeshProUGUI _gearType;
        private CanvasGroup _canvasGroup;
        public Gear gearModel;
        private bool _inAnimation;
        public Image shade;
        public Image image;
        public TextMeshProUGUI _chargeDesc;
        public TextMeshProUGUI _comboDesc;

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
            _gearDesc.text =  gearModel.desc;
            _gearType.text =  "Gear Type: " + gearModel.type;
            _chargeDesc.text = gearModel.chargeDesc;
            _comboDesc.text = gearModel.comboDesc;
            
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

