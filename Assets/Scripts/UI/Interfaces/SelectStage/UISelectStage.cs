using UnityEngine;
using Model;
using UI.Container;
using UnityEngine;
using Utility;

namespace UI.Interfaces.SelectStage
{
    public class UISelectStage : UIBase
    {
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
        
        public override void Open(object nextStageChoice) {
            base.Open(nextStageChoice);
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            ); 
            LoadStagePanel(nextStageChoice as int[]);
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
        
        
        public void LoadStagePanel(int[] nextStageIds) {
            var curPanelInd = 0;
            if (nextStageIds.Length > 5) {
                Debug.LogError("stages more than 5!");
                return;
            }

            foreach (var nextStage in nextStageIds) {
                var stagePanel = _panels[curPanelInd]; 
                stagePanel.OnClick += ChangeSelectedItemTo;
                stagePanel.Show = true; 
                stagePanel.Id = nextStage;
                curPanelInd++;
            }

            for (; curPanelInd < _panels.Length; curPanelInd++) _panels[curPanelInd].Show = false;
            container.UpdateLayout();
        }
        
        public void ConfirmButtonEvent()
        {
            GameManager.shared.game.LoadStage(_selected.Id);
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



