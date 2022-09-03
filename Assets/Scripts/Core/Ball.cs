using System;
using UnityEngine;

namespace Core{
    [RequireComponent(typeof(Collider2D))]
    
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

        private void Start(){
            _rectTransform = GetComponent<RectTransform>();
            _sprite = GetComponent<SpriteRenderer>();
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
        }

        public void OnBeingCircled(){
            Debug.Log("Circled!");
            currentState = State.Circled;
            _sprite.color = Color.blue;
        }
    }
}
