using System;
using UnityEngine;
using Utility.Extensions;

namespace Utility.Animation{

    public interface ILerpable<T>{
        T Add(T other);
        T Time(float k);
    }
    
    public abstract class Tween<T>: MonoBehaviour
    where T: ILerpable<T>{
        public T startValue;
        public T endValue;
        public float totalTime;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public bool repeat = false;
        public T CurrentValue{ get; private set; }
        private float _curTime = 0;
        public bool IsPaused{ get; private set; }
        public event Action<T> OnValueChanged;

        protected virtual void Start(){
            CurrentValue = startValue;
        }

        protected virtual void Update(){
            if (IsPaused) return;
            _curTime += Time.deltaTime;
            var i = curve.Evaluate(_curTime / totalTime);
            CurrentValue = Lerp(startValue, endValue, i);
            OnValueChange(CurrentValue);
            OnValueChanged?.Invoke(CurrentValue);
            if (_curTime.AlmostEquals(totalTime) || _curTime > totalTime){
                _curTime = 0;
                if (!repeat) IsPaused = true;
            }
        }

        private static T Lerp(T start, T end, float i){
            return start.Time(1 - i).Add(end.Time(i));
        }

        public void Pause(){
            IsPaused = true;
        }

        public void Play(){
            IsPaused = false;
        }

        public void Stop(){
            IsPaused = true;
            _curTime = 0;
        }

        protected abstract void OnValueChange(T current);
    }
}