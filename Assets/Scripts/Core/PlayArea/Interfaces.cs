using System;
using System.Collections.Generic;
using Core.PlayArea.TouchTracking;
using Model.Mechanics;
using UnityEngine;

namespace Core.PlayArea{

    public interface IPlayableObjectManager{
        
    }

    public abstract class PlayableObjectViewBase: MonoBehaviour{
        private static TouchTracker TouchTracker => GameManager.shared.touchTracker;
        
        public virtual void Update(){
            if (this is IMovable self){
                self.UpdatePosition();
            }
        }

        public virtual void OnMouseEnter(){
            if (this is ISliceable self){
                if (TouchTracker.isTracing){
                    TouchTracker.ContinueTracking();
                    self.OnSliced();
                }
            }
        }

        public virtual void OnMouseDown(){
            if (this is ISliceable self){
                TouchTracker.StartTracking();
                self.OnSliced();
            }
        }

        public void OnMouseUp(){
            if (this is ISliceable self){
                TouchTracker.StopTracking();
            }
        }
    }

    public interface ISliceable{
        // public Collider2D Collider2D{ get; }
        // public float Radius{ get; }
        public void OnSliced();
    }

    public interface ICircleable{
        public Collider2D Collider2D{ get; }
        public void OnCircled();
    }

    public interface IMovable{
        Vector2 Velocity{ get; set; }
        float VelocityMultiplier{ get; set; }
        void UpdatePosition();
    }
}