using System;
using System.Collections;
using UnityEngine;

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
    }
}