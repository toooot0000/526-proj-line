using BackendApi;
using Model;
using TMPro;
using UI.Container;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI{
    public class UISelectGear: UIBase{
        private bool _inAnimation = false;
        private CanvasGroup _canvasGroup;
        public UIContainerFlexBox panel;
        public GameObject gearPanelPrefab;
        public int selectedId = -1;
        public GameObject[] itemPanels = new GameObject[3];
        public Gear[] items = new Gear[3];
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
            LoadGearPanel(GameManager.shared.game.currentStage.bonusGears);
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
            int cnt = 0;
            foreach (var gear in gears)
            {
                print(cnt + " : " + gear.name);
                // Instance GearPanel
                var gearPanel = Instantiate(gearPanelPrefab, panel.transform);
                gearPanel.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(gear.imgPath);
                gearPanel.GetComponentInChildren<Image>().color = Color.green;
                gearPanel.GetComponentInChildren<TextMeshProUGUI>().text = gear.desc;
                print("desc:"+gear.desc);
                gearPanel.GetComponent<UIGearPanel>().id = cnt++;
                gearPanel.GetComponent<UIGearPanel>().OnClick += (game,id) =>
                {
                    print("click : " + id);
                    ChangeSelectedItemTo(id);
                };
                itemPanels[cnt - 1] = gearPanel;
                items[cnt - 1] = gear;
            }
            panel.UpdateLayout();
        }

        public void ConfirmButtonEvent() {
            StartCoroutine(CoroutineUtility.Delayed(GameManager.shared.game.GoToNextStage));
            GameManager.shared.game.player.AddGear(items[selectedId]);
            EventLogger.serverURL = "http://localhost:8080/";
            EventLogger.Shared.Log(new EventItemInteract()
            {
                itemId = items[selectedId].id,
                status = "obtained",
                count  = 1
            });
            Close();
        }

        public void ChangeSelectedItemTo(int id)
        {
            if (selectedId == id)
                return;
            if (selectedId == -1)
            {
                selectedId = id;
                //do the highlight
                itemPanels[id].GetComponent<UIGearPanel>().highLight.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                //undo the last highlight
                itemPanels[selectedId].GetComponent<UIGearPanel>().highLight.GetComponent<CanvasGroup>().alpha = 0;
                selectedId = id;
                //do the highlight
                itemPanels[id].GetComponent<UIGearPanel>().highLight.GetComponent<CanvasGroup>().alpha = 1;
            }
        }
    }
}