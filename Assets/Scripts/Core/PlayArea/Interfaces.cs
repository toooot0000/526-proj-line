using System;
using System.Collections.Generic;
using System.Linq;
using Core.PlayArea.TouchTracking;
using Model;
using Model.Mechanics;
using Tutorial;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea{

    public interface IPlayableViewManager {
        public IEnumerable<PlayableObjectViewBase> GetAllViews();
    }


    public abstract class PlayableViewManager<TModel> : MonoBehaviour, IPlayableViewManager
    where TModel: GameModel, IPlayableObject{
        protected readonly List<PlayableObjectViewWithModel<TModel>> views = new();

        public virtual PlayableObjectViewWithModel<TModel> Place(TModel model){
            var curItem = views.FirstNotActiveOrNew(GenerateNewObject);
            curItem.Model = model;
            curItem.gameObject.SetActive(true);
            curItem.Manager = this;
            GameManager.shared.playAreaManager.SetPlayableViewPosition(curItem);
            return curItem;
        }
        protected abstract PlayableObjectViewWithModel<TModel> GenerateNewObject();
        public virtual void Remove(PlayableObjectViewWithModel<TModel> view){
            views.Remove(view);
        }
        public virtual  IEnumerable<PlayableObjectViewBase> GetAllViews(){
            return views;
        }

        public virtual IEnumerable<PlayableObjectViewBase> GetActiveViews(){
            return GetAllViews().Where(playableObjectViewBase => playableObjectViewBase.gameObject.activeSelf);
        }
        public virtual void Start(){
            GameManager.shared.playAreaManager.RegisterManager(this);
        }
    }
    
    public abstract class PlayableObjectViewBase : MonoBehaviour, ITutorialControllable{
        private static TouchTracker TouchTracker => GameManager.shared.touchTracker;
        public bool IsInTutorial{ get; private set; }
        public bool tutorIsSliceable = true;
        public bool tutorIsCircleable = true;
        public bool tutorCanMove = true;

        public virtual void Update(){
            if (this is IForceableView forceableView){
                if(!IsInTutorial || tutorCanMove) forceableView.UpdateVelocity();
            }

            if (this is IMovableView self){
                if(!IsInTutorial || tutorCanMove) self.UpdatePosition();
            }
        }

        public virtual void OnMouseEnter(){
            if (this is ISliceableView self && TouchTracker.isTracing){
                TouchTracker.ContinueTracking();
                if(!IsInTutorial || tutorIsSliceable) self.OnSliced();
            }
        }

        public virtual void OnMouseDown(){
            if (this is ISliceableView self){
                TouchTracker.StartTracking();
                if(!IsInTutorial || tutorIsSliceable) self.OnSliced();
            }
        }

        public virtual void OnMouseUp(){
            if (this is ISliceableView self){
                StartCoroutine(TouchTracker.OnMouseUp());
            }
        }

        public static void UpdatePosition(PlayableObjectViewBase view){
            if (view is not IMovableView movable) return; 
            var rectTrans = (RectTransform)(view.transform);
            rectTrans.position += (Vector3)movable.Velocity * (Time.deltaTime * movable.VelocityMultiplier);
        }

        public static void UpdateVelocity(PlayableObjectViewBase view){
            if (view is not IForceableView forceableView) return;
            forceableView.Velocity += forceableView.Acceleration * Time.deltaTime;
        }

        public void HandOverControlTo(TutorialBase tutorial) {
            IsInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial) {
            IsInTutorial = false;
        }
    }

    public abstract class PlayableObjectViewWithModel<TModel> : PlayableObjectViewBase
    where TModel: GameModel, IPlayableObject{
        public abstract TModel Model{ set; get; }
        public virtual PlayableViewManager<TModel> Manager{ set; get; }
        
        public virtual void Split(){
            if (Model is ISplittable<TModel> splittable && this is ISplittableView splittableView){
                var newModel = splittable.Split();
                var splitView = Manager.Place(newModel);
                splitView.transform.localScale *= 0.5f;
                transform.localScale *= 0.5f;
                (splitView as IMovableView)!.Velocity = (splitView as IMovableView)!.Velocity.Rotated(15) * 2;
                (this as IMovableView)!.Velocity = (this as IMovableView)!.Velocity.Rotated(-15) * 2;
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

    public interface IForceableView : IMovableView{
        Vector2 Acceleration{ get; set; }
        public void UpdateVelocity();
    } 

    public interface IBlackHoleSuckableView: IForceableView{
        public void OnSucked();
    }

    public interface ISplittableView : IMovableView{
        public void Split();
    }

    public interface IOnPlayerFinishDrawing: IPlayableObjectViewProperty{
        public void OnPlayerFinishDrawing();
    }

    public interface IOnPlayerTurnEnd : IPlayableObjectViewProperty{
        public void OnPlayerTurnEnd();
    }
}