using Model;
using UnityEngine;
using Utility;
using Utility.Bezier;

namespace Core.PlayArea.Balls{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(BallConfig))]
    public class Ball : MonoBehaviour{
        public enum State{
            Free,
            Touched,
            Combo,
            Circled,
            Disabled,
            Charged,
            Flying,
        }

        public Vector2 velocity;
        public State currentState = State.Free;
        public AnimationCurve curve;

        private RectTransform _rectTransform;
        private SpriteRenderer _sprite;

        private global::Model.Ball Model => _config.modelBall;
        private Game _game;
        private BallConfig _config;


        private void Start(){
            _rectTransform = GetComponent<RectTransform>();
            _sprite = GetComponent<SpriteRenderer>();
            _game = GameManager.shared.game;
            _config = GetComponent<BallConfig>();
        }

        private void Update(){
            // if (Input.GetKeyDown(KeyCode.A)) {
            //     FlyToLocation(0.2f, Vector3.zero);
            // }
            if (currentState != State.Free) return;
            _rectTransform.position += (Vector3)velocity * Time.deltaTime;
        }
        
        public void OnHittingWall(Wall wall) {
            if (currentState != State.Free) return;
            velocity = new Vector2(velocity.x * wall.velocityChangeRate.x, velocity.y * wall.velocityChangeRate.y);
        }

        public void OnBeingTouched(){
            Debug.Log("Hit Touch!");
            if (((Gear)Model.parent).IsComboIng()) {
                currentState = State.Touched;
            }
            else {
                currentState = State.Combo;
                _sprite.color = Color.yellow;
            }
            _game.player.AddHitBall(_config.modelBall);
            
        }

        public void OnBeingCircled(){
            Debug.Log("Circled!");
            currentState = State.Circled;
            if (((Gear)Model.parent).IsCharged()) {
                currentState = State.Touched;
            }
            else {
                currentState = State.Charged;
                _sprite.color = Color.blue;
            }
            _game.player.AddCircledBall(_config.modelBall);
            
        }

        public void Reset() {
            
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
            
            var lerp = TweenUtility.Lerp(
                seconds: seconds,
                before: null,
                update: i => {
                    i = curve.Evaluate(i);
                    transform.position = BezierLerp.GetPoint(startWorldLocation, p1, p2, targetWorldLocation, i);
                    
                },
                finish: null
            );
            StartCoroutine(lerp());
        }
        
    }
}
