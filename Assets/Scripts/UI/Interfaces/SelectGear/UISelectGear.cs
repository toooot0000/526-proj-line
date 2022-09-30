using BackendApi;
using Model;
using UI.Container;
using UnityEngine;
using Utility;

namespace UI.Interfaces.SelectGear {
    public class UISelectGear : UIBase {
        public UIContainerFlexBox container;
        public Gear[] items;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIGearPanel[] _panels;
        private UIGearPanel _selected;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _panels = transform.GetComponentsInChildren<UIGearPanel>();
        }

        public override void Open(object gears) {
            base.Open();
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            items = gears as Gear[];
            LoadGearPanel();
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
        } // ReSharper disable Unity.PerformanceAnalysis
        public void LoadGearPanel() {
            var curPanelInd = 0;
            if (items.Length > 3) {
                Debug.LogError("Gears more than 3!");
                return;
            }

            foreach (var gear in items) {
                var gearPanel = _panels[curPanelInd];
                gearPanel.OnClick += ChangeSelectedItemTo;
                gearPanel.Show = true;
                gearPanel.Model = gear;
                curPanelInd++;
            }

            for (; curPanelInd < _panels.Length; curPanelInd++) _panels[curPanelInd].Show = false;
            container.UpdateLayout();
        }

        public void ConfirmButtonEvent() {
            StartCoroutine(CoroutineUtility.Delayed(GameManager.shared.game.GoToNextStage));
            GameManager.shared.game.player.AddGear(_selected.Model);
            EventLogger.Shared.Log(new EventItemInteract {
                itemId = _selected.Model.id,
                status = "obtained",
                count = 1
            });
            Close();
        }

        private void ChangeSelectedItemTo(UIGearPanel clickedPanel) {
            if (_selected == clickedPanel) return;
            if (_selected != null) _selected.highLight.enabled = false;
            clickedPanel.highLight.enabled = true;
            _selected = clickedPanel;
        }
    }
}