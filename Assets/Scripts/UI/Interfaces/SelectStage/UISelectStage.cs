using UnityEngine;
using Model;
using UI.Container;
using UnityEngine;
using Utility;

namespace UI.Interfaces.SelectStage
{
    public class UISelectStage : UIBase
    {
        
        public Stage currentStage;
        public UIContainerFlexBox container;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIStagePanel[] _panels;
        private UIStagePanel _selected;
        
        // Start is called before the first frame update
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _panels = transform.GetComponentsInChildren<UIStagePanel>();
        }
        
        public override void Open(object nextstagechoice) {
            base.Open();
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            ); 
            currentStage.nextStageChoice = nextstagechoice as Stage[];
            LoadStagePanel();
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
        
        
        public void LoadStagePanel() {
            var curPanelInd = 0;
            if (currentStage.nextStageChoice.Length > 5) {
                Debug.LogError("stages more than 5!");
                return;
            }

            foreach (var nextstage in currentStage.nextStageChoice) {
                var stagePanel = _panels[curPanelInd]; 
                stagePanel.OnClick += ChangeSelectedItemTo;
                stagePanel.Show = true; 
                stagePanel.Model = nextstage;
                curPanelInd++;
            }

            for (; curPanelInd < _panels.Length; curPanelInd++) _panels[curPanelInd].Show = false;
            container.UpdateLayout();
        }
        
        public void ConfirmButtonEvent()
        {
            GameManager.shared.game.LoadStage(_selected.Model.id);
            Close();
        }
        
        private void ChangeSelectedItemTo(UIStagePanel clickedPanel)
        {
            if (_selected == clickedPanel) return;
            if (_selected != null) _selected.highLight.enabled = false;
            clickedPanel.highLight.enabled = true;
            _selected = clickedPanel;
        }
        
    }
}



