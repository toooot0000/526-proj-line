using System;
using System.Collections;
using UnityEngine;

namespace Utility{
    public class CoroutineUtility{
        public static IEnumerator Delayed(Action action){
            yield return null;
            action();
        }

        public static IEnumerator Delayed(float seconds, Action action) {
            yield return new WaitForSeconds(seconds);
            action();
        }

        public static Func<IEnumerator> FadeOut(float time, Renderer renderer, Action finishCallBack) {
            IEnumerator Inner()
            {
                var currentColor = renderer.material.color;
                float curTime = 0;
                while (curTime < time) {
                    curTime += Time.deltaTime;
                    currentColor.a = 1 - curTime / time;
                    renderer.material.color = currentColor;
                    yield return null;
                }
            }
            return Inner;
        }

    }
}