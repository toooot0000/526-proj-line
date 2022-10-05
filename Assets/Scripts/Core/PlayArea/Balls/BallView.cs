using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
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
            Controlled,
        }

        public Vector2 velocity;
        public State currentState = State.Free;
        public AnimationCurve curve;
        public SpriteRenderer weaponIcon;

        private RectTransform _rectTransform;
        private SpriteRenderer _ballBg;

        public event BallViewEvent OnHitted;
        public event BallViewEvent OnCharged;

        public bool tutorCanBeHit = true;
        
        public bool tutorCanBeCircled = true;

        public global::Model.Ball Model{
            get => config.modelBall;
            set => config.modelBall = value;
        }

        private Game _game;
        public BallConfig config;

        public void UpdateConfig() => config.UpdateConfig();


        private void Start(){
            _rectTransform = GetComponent<RectTransform>();
            _ballBg = GetComponent<SpriteRenderer>();
            _game = GameManager.shared.game;
            config = GetComponent<BallConfig>();
        }

        private void Update(){
            if (currentState != State.Free) return;
            _rectTransform.position += (Vector3)velocity * Time.deltaTime;
        }
        
        public void OnHittingWall(Wall wall) {
            if (currentState != State.Free) return;
            velocity = new Vector2(velocity.x * wall.velocityChangeRate.x, velocity.y * wall.velocityChangeRate.y);
        }

        public void OnBeingTouched(){
            Debug.Log("Hit Touch!");
            if (currentState != State.Free && (currentState == State.Controlled && !tutorCanBeHit)) return;
            _game.player.AddHitBall(config.modelBall);
            if (((Gear)Model.parent).IsComboIng()) {
                currentState = State.Touched;
            }
            else {
                currentState = State.Combo;
                _ballBg.color = Color.yellow;
            }
            OnHitted?.Invoke(this);
        }

        public void OnBeingCircled(){
            Debug.Log("Circled!");
            if (currentState != State.Free || (currentState == State.Controlled && !tutorCanBeCircled)) return;
            _game.player.AddCircledBall(config.modelBall);
            currentState = State.Circled;
            if (((Gear)Model.parent).IsCharged()) {
                currentState = State.Touched;
            }
            else {
                currentState = State.Charged;
                _ballBg.color = Color.blue;
            }
            OnCharged?.Invoke(this);
        }

        public void ResetView(){
            currentState = State.Free;
        }

        public void FlyToLocation(float seconds, Vector3 targetWorldLocation) {
            currentState = State.Flying;
            var startWorldLocation = transform.position;
            var p1 = new Vector3() {
                x = startWorldLocation.x,
                y = (startWorldLocation.y + targetWorldLocation.y) / 2,
                z = startWorldLocation.z
            };
            var p2 = new Vector3() {
                x = (startWorldLocation.x + targetWorldLocation.x) / 2,
                y = targetWorldLocation.y,
                z = startWorldLocation.z
            };

            var bgStartColor = _ballBg.color;
            var bgEndColor = new Color(bgStartColor.r, bgStartColor.g, bgStartColor.b, 0);
            var iconStartColor = weaponIcon.color;
            var iconEndColor = new Color(iconStartColor.r, iconStartColor.g, iconStartColor.b, 0);
            
            var lerp = TweenUtility.Lerp(
                seconds: seconds,
                begin: null,
                update: i => {
                    i = curve.Evaluate(i);
                    transform.position = BezierLerp.GetPoint(startWorldLocation, p1, p2, targetWorldLocation, i);
                },
                complete: () => {
                    currentState = State.Hide;
                    // _ballBg.color = Color.Lerp(bgStartColor, bgEndColor, i);
                    _ballBg.color = bgEndColor;
                    // weaponIcon.color = Color.Lerp(iconStartColor, iconEndColor, i);
                    weaponIcon.color = iconEndColor;
                });
            StartCoroutine(lerp());
        }

        public void FadeOut(float seconds){
            currentState = State.Fading;
            var bgStartColor = _ballBg.color;
            var bgEndColor = new Color(bgStartColor.r, bgStartColor.g, bgStartColor.b, 0);
            var iconStartColor = weaponIcon.color;
            var iconEndColor = new Color(iconStartColor.r, iconStartColor.g, iconStartColor.b, 0);
            
            var lerp = TweenUtility.Lerp(
                seconds: seconds,
                begin: null,
                update: i => {
                    i = curve.Evaluate(i);
                    _ballBg.color = Color.Lerp(bgStartColor, bgEndColor, i);
                    weaponIcon.color = Color.Lerp(iconStartColor, iconEndColor, i);
                },
                complete: () => currentState = State.Hide
            );
            StartCoroutine(lerp());
        }

        public void ControlledByTutorial(TutorialBase tutorial){
            currentState = State.Controlled;
        }

        public void GainBackControl(TutorialBase tutorial){
            currentState = State.Hide;
        }

        public void TutorialSetPosition(Vector3 worldPosition){
            if (currentState != State.Controlled) return;
            transform.position = worldPosition;
        }
    }
}
