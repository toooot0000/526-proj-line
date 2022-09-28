using Model;
using TMPro;
using UI.Container;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI{
    public class UISelectGear: UIBase{
        private bool _inAnimation = false;
        private CanvasGroup _canvasGroup;
        public UIContainerFlexBox panel;
        public GameObject gearPanelPrefab;

        private void Start(){
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public override void Open(){
            
            base.Open();
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f, 
            () => _canvasGroup.alpha = 0, 
            i => _canvasGroup.alpha = i, 
            () => _inAnimation = false
            );
            StartCoroutine(coroutine());
        }

        public override void Close(){
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

            // GameManager.shared.game.LoadNewStage();
        }

        public void LoadGearPanel(Gear[] gears)
        {
            foreach (var gear in gears)
            {
                // Instance GearPanel
                var gearPanel = Instantiate(gearPanelPrefab, panel.transform);
                // gearPanel.GetComponentInChildren<Image>().color = Color.black;
                // gearPanel.GetComponentInChildren<TextMeshProUGUI>().text = gear.desc;
            }
            panel.UpdateLayout();
        }

        public void ConfirmButtonEvent() {
            StartCoroutine(CoroutineUtility.Delayed(GameManager.shared.game.GoToNextStage));
            Close();
        }
    }
}