using System;
using System.Collections.Generic;
using Core.PlayArea.TouchTracking;
using Model.Mechanics;
using UnityEngine;

namespace Core.PlayArea{

    public interface IPlayableViewManager {
        public IEnumerable<PlayableObjectViewBase> GetAllViews();
    }

    public abstract class PlayableObjectViewBase: MonoBehaviour{
        private static TouchTracker TouchTracker => GameManager.shared.touchTracker;
        
        public virtual void Update(){
            if (this is IBlackHoleSuckableView suckable){
                suckable.UpdateVelocity();
            }
            if (this is IMovableView self){
                self.UpdatePosition();
            }
        }

        public virtual void OnMouseEnter(){
            if (this is ISliceableView self){
                if (TouchTracker.isTracing){
                    TouchTracker.ContinueTracking();
                    self.OnSliced();
                }
            }
        }

        public virtual void OnMouseDown(){
            if (this is ISliceableView self){
                TouchTracker.StartTracking();
                self.OnSliced();
            }
        }

        public void OnMouseUp(){
            if (this is ISliceableView self){
                StartCoroutine(TouchTracker.OnMouseUp());
            }
        }
    }

    public interface IPlayableObjectViewProperty { }

    public interface ISliceableView: IPlayableObjectViewProperty{
        public void OnSliced();
    }

    public interface ICircleableView: IPlayableObjectViewProperty{
        public void OnCircled();
    }

    public interface IMovableView: IPlayableObjectViewProperty{
        Vector2 Velocity{ get; set; }
        float VelocityMultiplier{ get; set; }
        void UpdatePosition();
    }

    public interface IBlackHoleSuckableView: IMovableView{
        Vector2 Acceleration{ get; set; }
        public void OnSucked();
        public void UpdateVelocity();
    }

    public interface ISplitable : IMovableView{
        
    }
}