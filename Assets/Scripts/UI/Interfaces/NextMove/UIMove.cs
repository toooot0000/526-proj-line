using System.Linq;
using Core.DisplayArea.Stage.Enemy;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Interfaces.NextMove
{
    public class UIMove:UIBase
    {
        public TextMeshProUGUI moveTitle;
        public TextMeshProUGUI moveDesc;
        public Intention moveModel;
        public IntentionDisplayer.IntentionPair[] pairs;
        public Image moveIcon;
        private Transform _parent;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private bool isDestroyed;
        private float timer;

        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            timer = 3.0f;

        }
        
        public void Update()
        {
            if (!isDestroyed) { 
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    Close();
                }
            }
        }
        

        public override void Open(object model){
            //moveModel = (IntentionDisplayer.IntentionInfo) model;

            moveModel = (Intention)model;
            base.Open(model);
            _inAnimation = true;

            moveTitle.text = moveModel.name;
            moveDesc.text = moveModel.desc;
            moveIcon.sprite = Resources.Load<Sprite>(moveModel.imgPth);
            // moveTitle.text = moveModel.intention.ToString();
            // if (moveModel.number != 0)
            // {
            //     moveDesc.text = moveModel.intention + ":" + moveModel.number;
            // }
            // else
            // {
            //     moveDesc.text = moveModel.intention.ToString();
            // }
            //
            // moveIcon.sprite = pairs.First(p => p.intention == moveModel.intention).sprite;
            
            
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            StartCoroutine(coroutine());
            
        }

        public void Close() {
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