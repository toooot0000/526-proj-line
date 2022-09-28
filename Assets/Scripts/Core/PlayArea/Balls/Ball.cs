using Model;
using UnityEngine;
using Utility;

namespace Core.PlayArea.Balls{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(BallConfig))]
    public class Ball : MonoBehaviour{
        public enum State{
            Free,
            Touched,
            Circled,
            Disabled,
        }

        public Vector2 velocity;
        public State currentState = State.Free;

        private RectTransform _rectTransform;
        private SpriteRenderer _sprite;

        private global::Model.Ball _model;
        private Game _game;
        

        private void Start(){
            _rectTransform = GetComponent<RectTransform>();
            _sprite = GetComponent<SpriteRenderer>();
            _game = GameManager.shared.game;
            _model = GetComponent<BallConfig>().modelBall;
        }

        private void Update(){
            if (currentState != State.Free) return;
            _rectTransform.position += (Vector3)velocity * Time.deltaTime;
        }
        
        public void OnHittingWall(Wall wall){
            velocity = new Vector2(velocity.x * wall.velocityChangeRate.x, velocity.y * wall.velocityChangeRate.y);
        }

        public void OnBeingTouched(){
            Debug.Log("Hit Touch!");
            _sprite.color = Color.yellow;
            currentState = State.Touched;
            _game.player.AddHitBall(_model);
            var fun = CoroutineUtility.FadeOut(0.2f, GetComponent<Renderer>(), () => Destroy(this));
            StartCoroutine(fun());
        }

        public void OnBeingCircled(){
            Debug.Log("Circled!");
            currentState = State.Circled;
            _sprite.color = Color.blue;
            _game.player.AddCircledBall(_model);
            var fun = CoroutineUtility.FadeOut(0.2f, GetComponent<Renderer>(), () => Destroy(this));
            StartCoroutine(fun());
        }
    }
}
