using UnityEngine;
using Utility;
using Utility.Loader;

namespace UI.Interfaces.SelectStage
{
    public class StagePanel : UIBase
    {
        
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        public SelectStage[] _panels;

        private void Start(){
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1;
            _panels = transform.GetComponentsInChildren<SelectStage>();
        }
        
        public override void Open(object stageChoice) {
            base.Open(stageChoice);
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            ); 
            LoadStagePanel();
            StartCoroutine(coroutine());
        }

        public void LoadStagePanel() {
            var curPanelInd = 0;
            var stageIds = CsvLoader.Load("Configs/stages").Keys;

            foreach (var stage in stageIds) {
              
                var stagePanel = _panels[curPanelInd];
                stagePanel.OnClick += GotoStage;
                stagePanel.Show = true; 
                stagePanel.Id = stage;
                curPanelInd++;
            }
            for (; curPanelInd < _panels.Length; curPanelInd++) _panels[curPanelInd].Show = false;
        }

        public void GotoStage(SelectStage _clickedPanel)
        {
            GameManager.shared.GotoStage(_clickedPanel._id);
            Close();
        }
        
        public override void Close(){
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

    }

}
