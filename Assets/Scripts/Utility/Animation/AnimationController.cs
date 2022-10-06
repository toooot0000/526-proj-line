using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Utility.Animation{
    public class AnimationController<T> : MonoBehaviour
        where T : Enum{
        public delegate void AnimationCallback(T currentAnimation);

        private readonly Dictionary<T, float> _times = new();
        private Animator _animator;

        private T _currentAnimation;

        private void Awake(){
            _animator = GetComponent<Animator>();
            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
                try{
                    _times[EnumUtility.GetValue<T>(clip.name)] = clip.length + 0.02f;
                } catch{
                    Debug.Log($"Enum not has the animation name: {clip.name}");
                }
        }

        public void Play(T anim){
            _animator.Play(GetDescription(anim));
        }

        public void Play(T anim, Action completeCallback){
            Play(anim);
            StartCoroutine(CoroutineUtility.Delayed(_times[anim], completeCallback));
        }

        public IEnumerator PlayUntilComplete(T anim){
            Play(anim);
            yield return new WaitForSeconds(_times[anim]);
        }

        public void Play(T anim, float seconds, Action callback){
            Play(anim);
            StartCoroutine(CoroutineUtility.Delayed(seconds, callback));
        }


        private static string GetDescription(T value){
            var field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute
                attribute
                ? value.ToString()
                : attribute.Description;
        }
    }
}