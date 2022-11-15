using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace UI.Interfaces{

    public interface IUISetUpOptions<in T> where T: UIBase{
        void ApplyOptions(T uiBase);
    }
    
    public abstract class UIBase : MonoBehaviour{

        public static IEnumerator FadeIn(CanvasGroup canvasGroup, Action completion = null){
            var coroutine = TweenUtility.Lerp(0.2f,
                () => canvasGroup.alpha = 0,
                i => canvasGroup.alpha = i,
                completion
            );
            yield return coroutine();
        }

        public static IEnumerator FadeOut(CanvasGroup canvasGroup, Action completion = null){
            var coroutine = TweenUtility.Lerp(0.2f,
                () => canvasGroup.alpha = 1,
                i => canvasGroup.alpha = 1 - i,
                completion);
            yield return coroutine();
        }
        
        public virtual string Name => "Base";

        public virtual void Open(object nextStageChoice){
            OnOpen?.Invoke(this);
        }

        public virtual void Close(){
            OnClose?.Invoke(this);
        }

        public event UINormalEvent OnOpen;
        public event UINormalEvent OnClose;
    }
}