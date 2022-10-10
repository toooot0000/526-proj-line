using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utility{
    public interface ITweenArithmetic<T>{
        T Add(T other);
        T Sub(T other);
        T Mul(float other);
    }

    public struct Tween<T>
        where T : ITweenArithmetic<T>{
        private readonly T _start;
        private T _growth;
        public readonly float totalTime;
        private readonly AnimationCurve _curve;

        public Tween(T start, T end, float time, AnimationCurve curve){
            _start = start;
            _growth = end.Sub(start);
            totalTime = time;
            _curve = curve;
        }

        public T GetCurrentValue(float time){
            time = Mathf.Clamp(time, 0, totalTime);
            return _growth.Mul(_curve.Evaluate(time / totalTime)).Add(_start);
        }
    }

    public static class TweenUtility{
        public static IEnumerator MakeEnumerator<T>(Tween<T> tween, Action<T> callback)
            where T : ITweenArithmetic<T>{
            // return new TweenEnumerator<T>(tween);
            var time = 0.0f;
            while (time < tween.totalTime){
                yield return null;
                time += Time.deltaTime;
                callback(tween.GetCurrentValue(time));
            }
        }

        public static Func<IEnumerator> Lerp(float seconds, Action begin, Action<float> update, Action complete){
            IEnumerator Inner(){
                begin?.Invoke();
                float curTime = 0;
                while (curTime < seconds){
                    curTime += Time.deltaTime;
                    update?.Invoke(curTime / seconds);
                    yield return null;
                }

                complete?.Invoke();
            }

            return Inner;
        }

        public static IEnumerator Move(float seconds, Transform transform, Vector3 start, Vector3 end, AnimationCurve curve = null){
            return Lerp(
                seconds: seconds,
                begin: () => { transform.position = start; },
                update: (i) => {
                    if (curve != null) i = curve.Evaluate(i);
                    transform.position = Vector3.Lerp(start, end, i);
                },
                complete: () => { transform.position = end;}
            )();
        }
        
        public static IEnumerator Move(float seconds, RectTransform transform, Vector2 anchoredStart, Vector2 anchoredEnd, AnimationCurve curve = null){
            return Lerp(
                seconds: seconds,
                begin: () => { transform.anchoredPosition = anchoredStart; },
                update: (i) => {
                    if (curve != null) i = curve.Evaluate(i);
                    transform.anchoredPosition = Vector2.Lerp(anchoredStart, anchoredEnd, i);
                },
                complete: () => { transform.anchoredPosition = anchoredEnd;}
            )();
        }
        
        public static IEnumerator Fade(float seconds, Image image, Color startColor, Color endColor, AnimationCurve curve = null){
            return Lerp(
                seconds: seconds,
                begin: () => { image.color = startColor; },
                update: (i) => {
                    if (curve != null) i = curve.Evaluate(i);
                    image.color = Color.Lerp(startColor, endColor, i);
                },
                complete: () => { image.color = endColor;}
            )();
        }
    }
}