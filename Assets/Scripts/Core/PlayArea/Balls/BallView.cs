using Model;
using Tutorials;
using UnityEngine;
using Utility;
using Utility.Bezier;

namespace Core.PlayArea.Balls{
    public delegate void BallViewEvent(BallView view);

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(BallConfig))]
    public class BallView : MonoBehaviour, ITutorialControllable{
        public enum State{
            Free,
            Touched,
            Combo,
            Circled,
            Charged,
            Flying,
            Fading,
            Hide,
            Controlled
        }

        public Vector2 velocity;
        private State _currentState = State.Free;

        public State CurrentState{
            set{
                _currentState = value;
                switch (value){
                    case State.Combo:
                        _ballBg.color = Color.yellow;
                        break;
                    case State.Charged:
                        _ballBg.color = Color.magenta;
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
        private SpriteRenderer _ballBg;

        private Game _game;

        private RectTransform _rectTransform;

        public Ball Model{
            get => config.modelBall;
            set => config.modelBall = value;
        }


        private void Start(){
            _rectTransform = GetComponent<RectTransform>();
            _ballBg = GetComponent<SpriteRenderer>();
            _game = GameManager.shared.game;
            config = GetComponent<BallConfig>();
        }

        private void Update(){
            if (CurrentState != State.Free) return;
            _rectTransform.position += (Vector3)velocity * Time.deltaTime;
        }

        public void ControlledByTutorial(TutorialBase tutorial){
            CurrentState = State.Controlled;
        }

        public void GainBackControl(TutorialBase tutorial){
            CurrentState = State.Hide;
        }

        public event BallViewEvent OnHitted;
        public event BallViewEvent OnCharged;

        public void UpdateConfig(){
            config.UpdateConfig();
        }

        public void OnHittingWall(Wall wall){
            if (CurrentState != State.Free) return;
            velocity = new Vector2(velocity.x * wall.velocityChangeRate.x, velocity.y * wall.velocityChangeRate.y);
        }

        public void OnBeingTouched(){
            if (CurrentState != State.Free && CurrentState == State.Controlled && !tutorCanBeHit) return;
            _game.player.AddHitBall(config.modelBall);
            OnHitted?.Invoke(this);
        }

        public void OnBeingCircled(){
            if (CurrentState != State.Free || (CurrentState == State.Controlled && !tutorCanBeCircled)) return;
            _game.player.AddCircledBall(config.modelBall);
            OnCharged?.Invoke(this);
        }

        public void ResetView(){
            CurrentState = State.Free;
        }

        public void FlyToLocation(float seconds, Vector3 targetWorldLocation){
            CurrentState = State.Flying;
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

            var bgStartColor = _ballBg.color;
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
                    _ballBg.color = bgEndColor;
                    weaponIcon.color = iconEndColor;
                });
            StartCoroutine(lerp());
        }

        public void FadeOut(float seconds){
            CurrentState = State.Fading;
            var bgStartColor = _ballBg.color;
            var bgEndColor = new Color(bgStartColor.r, bgStartColor.g, bgStartColor.b, 0);
            var iconStartColor = weaponIcon.color;
            var iconEndColor = new Color(iconStartColor.r, iconStartColor.g, iconStartColor.b, 0);

            var lerp = TweenUtility.Lerp(
                seconds,
                null,
                i => {
                    i = curve.Evaluate(i);
                    _ballBg.color = Color.Lerp(bgStartColor, bgEndColor, i);
                    weaponIcon.color = Color.Lerp(iconStartColor, iconEndColor, i);
                },
                () => CurrentState = State.Hide
            );
            StartCoroutine(lerp());
        }

        public void TutorialSetPosition(Vector3 worldPosition){
            if (CurrentState != State.Controlled) return;
            transform.position = worldPosition;
        }
    }
}