using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using Utility.Extensions;

namespace Utility.Animation{
    public class AnimationController<T> : MonoBehaviour
    where T: Enum{
        private Animator _animator;

        public delegate void AnimationCallback(T currentAnimation);

        public event AnimationCallback OnAnimationComplete;

        private bool _isAnimating = false;

        private T _currentAnimation;

        private readonly Dictionary<T, float> _times = new();

        private void Awake(){
            _animator = GetComponent<Animator>();
            foreach (var clip in _animator.runtimeAnimatorController.animationClips){
                try{
                    _times[EnumUtility.GetValue<T>($"Trg{clip.name}")] = clip.length;
                } catch {
                    Debug.Log($"Enum not has the animation name: {clip.name}");
                }
            }
        }

        public void PlayAnimation(T anim){
            _animator.SetTrigger(GetDescription(anim));
        }

        public void PlayAnimation(T anim, Action completeCallback){
            PlayAnimation(anim);
            StartCoroutine(CoroutineUtility.Delayed(_times[anim], completeCallback));
        }
        

        
        static string GetDescription(T value){
            var field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
        }
        
    }
}