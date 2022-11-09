using System;
using Core.DisplayArea.Stage;
using UnityEngine;
using Utility;
using Utility.Bezier;

namespace Core.PlayArea.Mines{
    public enum MineState{
        Idle,
        Triggered,
        Removed
    }
    
    public class MineView: PlayableObjectViewBase, ICircleableView, ISliceableView, IBlackHoleSuckableView{
        public MineAnimationController animationController;
        private Model.Mechanics.PlayableObjects.Mine _model;
        public Model.Mechanics.PlayableObjects.Mine Model{
            set{
                _model = value;
                Velocity =  _model.Velocity * Vector2Utility.RandomDirection;
            }
            get => _model;
        }
        public StageManager stageManager;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public float flyTime = 0.2f;
        public MineState state = MineState.Idle;
        public new SpriteRenderer renderer;
        private float _velocity;
        public Vector2 Velocity{ get; set; } = Vector2.zero;

        public float VelocityMultiplier{
            get => Model.VelocityMultiplier; 
            set => Model.VelocityMultiplier = value;
        }
        
        public void Init() {
            state = MineState.Idle;
            renderer.color = Color.white;
            animationController.Play(MineAnimation.Idle);
        }

        public void OnCircled(){
            if (state != MineState.Idle) return;
            state = MineState.Removed;
            animationController.Play(MineAnimation.Disappear, () => {
                Model.OnCircled().Execute();
                gameObject.SetActive(false);
            });
        }
        
        
        private void FlyToLocation(float seconds, Vector3 targetWorldLocation, Action completion){
            var startWorldLocation = transform.position;
            var p1 = new Vector3{
                x = startWorldLocation.x,
                y = (startWorldLocation.y + targetWorldLocation.y) / 2,
                z = startWorldLocation.z
            };
            var p2 = new Vector3{
                x = (startWorldLocation.x + targetWorldLocation.x) / 2,
                y = targetWorldLocation.y,
                z = startWorldLocation.z
            };
            targetWorldLocation.z = startWorldLocation.z;
            var lerp = TweenUtility.Lerp(
                seconds,
                null,
                i => {
                    i = curve.Evaluate(i);
                    transform.position = BezierLerp.GetPoint(startWorldLocation, p1, p2, targetWorldLocation, i);
                },
                completion
                );
            StartCoroutine(lerp());
        }
        public void UpdatePosition(){
            if (state != MineState.Idle) return;
            var rectTrans = (RectTransform)transform;
            rectTrans.position += (Vector3)Velocity * (Time.deltaTime * VelocityMultiplier);
        }
        
        public void OnSliced(){
            if (state != MineState.Idle) return;
            state = MineState.Triggered;
            FlyToLocation(flyTime, stageManager.playerView.transform.position, () => {
                animationController.Play(MineAnimation.Explosion, () => {
                    Model.OnSliced().Execute();
                    gameObject.SetActive(false);
                });
            });
        }

        public Vector2 Acceleration{ get; set; }
        public void OnSucked(){
            OnSliced();
        }

        public void UpdateVelocity(){
            Velocity += Acceleration * Time.deltaTime;
        }
    }
}