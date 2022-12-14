using System.Linq;
using UnityEngine;
using Model;
using TMPro;
using Tutorial;
using Tutorial.Tutorials.StageTypes;
using UI.Container;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Interfaces.SelectStage
{
    public class UISelectStage : UIBase, ITutorialControllable
    {
        public UIContainerFlexBox container;
        public Button confirmButton;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIStagePanel[] _panels;
        private UIStagePanel _selected;
        
        private bool _isInTutorial = false;

        public event TutorialControllableEvent OnConfirmClicked;

        // public override string PrefabName{ get; } = "UISelectStage";

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _panels = transform.GetComponentsInChildren<UIStagePanel>();
        }

        // public static UISelectStage Open(int[] nextStageIds){
        //     return UIBase.Open<UISelectStage, int[]>(nextStageIds);
        // }

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
            if (GameManager.shared.game.currentStage.id == 1){
                GameManager.shared.tutorialManager.LoadTutorial<UITutorialStageTypes>();
            }
        }

        public override void Close() {
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 1,
                i => _canvasGroup.alpha = 1 - i,
                () => {
                    _inAnimation = false;
                    base.Close();
                    foreach (var uiStagePanel in _panels){
                        Destroy(uiStagePanel.gameObject);
                    }
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
            ChangeSelectedItemTo(_panels[0]);
        }
        
        public void ConfirmButtonEvent(){
            if (_selected == null) return;
            OnConfirmClicked?.Invoke(null);
            GameManager.shared.GotoStage(_selected.Id);
            Close();
        }
        
        private void ChangeSelectedItemTo(UIStagePanel clickedPanel){
            if (_isInTutorial) return;
            if (_selected == clickedPanel) return;
            if (_selected != null) _selected.highLight.enabled = false;
            clickedPanel.highLight.enabled = true;
            _selected = clickedPanel;
        }

        public UIStagePanel GetFirstPanel() => _panels.Length > 0 ? _panels[0] : null;

        public UIStagePanel GetPanelOfId(int id) => _panels.First(p => p.Id == id);
        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
        }
    }
}



