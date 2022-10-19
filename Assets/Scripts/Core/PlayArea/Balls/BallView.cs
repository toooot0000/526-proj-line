using System;
using System.Collections;
using Model;
using Tutorial;
using UnityEngine;
using Utility;
using Utility.Bezier;

namespace Core.PlayArea.Balls{
    public delegate void BallViewEvent(BallView view);

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(BallConfig))]
    public class BallView : MonoBehaviour, ITutorialControllable{
        public event Action<BallView> OnMouseEnterBall;
        public event Action OnMouseExitBall;
        public event Action OnMouseUpBall;
        
        public enum State{
            Free,
            Touched,
            Combo,
            Circled,
            Charged,
            Hide,
            Controlled,
            Animating,
        }

        public Vector2 velocity;
        private State _currentState = State.Free;

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
        public AnimationCurve curve;
        public SpriteRenderer weaponIcon;

        public bool tutorCanBeHit = true;

        public bool tutorCanBeCircled = true;
        public BallConfig config;
        private SpriteRenderer BallBg => config.bg;

        private Game _game;

        private RectTransform _rectTransform;

        public Ball Model{
            get => config.modelBall;
            set => config.modelBall = value;
        }


        private void Start(){
            _rectTransform = GetComponent<RectTransform>();
            _game = GameManager.shared.game;
            config = GetComponent<BallConfig>();
        }

        private void Update(){
            if (CurrentState != State.Free) return;
            _rectTransform.position += (Vector3)velocity * Time.deltaTime;
        }

        public void HandOverControlTo(TutorialBase tutorial){
            CurrentState = State.Controlled;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            CurrentState = State.Hide;
        }

        public event BallViewEvent OnHitted;
        public event BallViewEvent OnCharged;

        public void UpdateConfig(){
            config.ResetView();
        }

        public void OnHittingWall(Wall wall){
            if (CurrentState != State.Free) return;
            velocity = new Vector2(velocity.x * wall.velocityChangeRate.x, velocity.y * wall.velocityChangeRate.y);
        }

        public void OnBeingTouched(){
            if (CurrentState != State.Free && (CurrentState != State.Controlled || !tutorCanBeHit)) return;
            _game.player.AddHitBall(config.modelBall);
            OnHitted?.Invoke(this);
        }

        public void OnBeingCircled(){
            if (CurrentState != State.Free && (CurrentState != State.Controlled || !tutorCanBeCircled)) return;
            _game.player.AddCircledBall(config.modelBall);
            OnCharged?.Invoke(this);
        }

        private void OnMouseEnter(){
            OnMouseEnterBall?.Invoke(this);
        }

        private void OnMouseUp(){
            OnMouseUpBall?.Invoke();
        }

        private void OnMouseExit(){
            OnMouseExitBall?.Invoke();
        }

        public void ResetView(){
            CurrentState = State.Free;
        }

        public void FlyToLocation(float seconds, Vector3 targetWorldLocation){
            CurrentState = State.Animating;
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
    }
}