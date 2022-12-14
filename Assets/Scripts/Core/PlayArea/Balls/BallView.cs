using System;
using Model;
using Model.Mechanics;
using Model.Mechanics.PlayableObjects;
using Tutorial;
using UnityEngine;
using Utility;
using Utility.Bezier;

namespace Core.PlayArea.Balls{
    public delegate void BallViewEvent(BallView view);

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(BallConfig))]
    public class BallView : PlayableObjectViewWithModel<Ball>, ITutorialControllable, ISliceableView, ICircleableView, IBlackHoleSuckableView{
        public enum State{
            Free,
            Touched,
            Combo,
            Circled,
            Charged,
            Hide,
            Controlled,
            Animating
        }

        public Vector2 Velocity{ set; get; }

        public float VelocityMultiplier{ set; get; } = 1f;

        public AnimationCurve curve;
        public SpriteRenderer weaponIcon;

        public bool tutorCanBeHit = true;

        public bool tutorCanBeCircled = true;
        public BallConfig config;
        private State _currentState = State.Free;
        private float _velocity;

        public State CurrentState{
            set{
                _currentState = value;
                switch (value){
                    case State.Combo:
                        BallBg.sprite = config.sprCombo;
                        break;
                    case State.Charged:
                        BallBg.sprite = config.sprCharge;
                        break;
                }
            }
            get => _currentState;
        }

        private SpriteRenderer BallBg => config.bg;

        public override Ball Model{
            get => config.modelBall;
            set => config.modelBall = value;
        }

        public event BallViewEvent OnBallSliced;
        public event BallViewEvent OnBallCircled;

        private void Start(){
            config = GetComponent<BallConfig>();
        }
        
        public void HandOverControlTo(TutorialBase tutorial){
            CurrentState = State.Controlled;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            CurrentState = State.Hide;
        }
        
        public void UpdateConfig(){
            config.ResetView();
        }

        public void OnSliced(){
            if (CurrentState != State.Free && (CurrentState != State.Controlled || !tutorCanBeHit)) return;
            Model.OnSliced().Execute();
            OnBallSliced?.Invoke(this);
        }

        public void OnCircled(){
            if (CurrentState != State.Free && (CurrentState != State.Controlled || !tutorCanBeCircled)) return;
            Model.OnCircled().Execute();
            OnBallCircled?.Invoke(this);
        }

        public void ResetView(){
            CurrentState = State.Free;
        }

        public void FlyToLocation(float seconds, Vector3 targetWorldLocation){
            CurrentState = State.Animating;
            var startWorldLocation = transform.position;
            targetWorldLocation.z = startWorldLocation.z;
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

            var bgStartColor = BallBg.color;
            var bgEndColor = new Color(bgStartColor.r, bgStartColor.g, bgStartColor.b, 0);
            var iconStartColor = weaponIcon.color;
            var iconEndColor = new Color(iconStartColor.r, iconStartColor.g, iconStartColor.b, 0);

            var lerp = TweenUtility.Lerp(
                seconds,
                null,
                i => {
                    i = curve.Evaluate(i);
                    transform.position = BezierLerp.GetPoint(startWorldLocation, p1, p2, targetWorldLocation, i);
                },
                () => {
                    CurrentState = State.Hide;
                    BallBg.color = bgEndColor;
                    weaponIcon.color = iconEndColor;
                });
            StartCoroutine(lerp());
        }

        public void FadeOut(float seconds){
            CurrentState = State.Animating;
            var bgStartColor = BallBg.color;
            var bgEndColor = new Color(bgStartColor.r, bgStartColor.g, bgStartColor.b, 0);
            var iconStartColor = weaponIcon.color;
            var iconEndColor = new Color(iconStartColor.r, iconStartColor.g, iconStartColor.b, 0);

            var lerp = TweenUtility.Lerp(
                seconds,
                null,
                i => {
                    i = curve.Evaluate(i);
                    BallBg.color = Color.Lerp(bgStartColor, bgEndColor, i);
                    weaponIcon.color = Color.Lerp(iconStartColor, iconEndColor, i);
                },
                () => CurrentState = State.Hide
            );
            StartCoroutine(lerp());
        }

        public void TutorialSetPosition(Vector2 worldPosition){
            if (CurrentState != State.Controlled) return;
            var transform1 = transform;
            transform1.position = new Vector3(worldPosition.x, worldPosition.y, transform1.position.z);
        }

        public void UpdatePosition(){
            if (CurrentState != State.Free) return;
            var rectTrans = (RectTransform)transform;
            rectTrans.position += (Vector3)Velocity * (Time.deltaTime * VelocityMultiplier);
        }

        public Vector2 Acceleration{ get; set; }
        public void OnSucked(){
            FadeOut(0.2f);
        }

        public void UpdateVelocity(){
            Velocity += Acceleration * Time.deltaTime;
        }
    }
}