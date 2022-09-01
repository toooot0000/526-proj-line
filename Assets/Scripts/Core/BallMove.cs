using System;
using UnityEngine;

namespace Core{
    [RequireComponent(typeof(Collider2D))]
    public class BallMove : MonoBehaviour{

        public Vector2 velocity;

        private RectTransform _rectTransform;

        private void Start(){
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update(){
            _rectTransform.position += (Vector3)velocity * Time.deltaTime;
        }
        
        
        private void OnTriggerEnter2D(Collider2D col){
            if (col.gameObject.layer == LayerMask.NameToLayer("Wall")){
                var wall = col.gameObject.GetComponent<WallBounce>();
                velocity = new Vector2(velocity.x * wall.velocityChangeRate.x, velocity.y * wall.velocityChangeRate.y);
            } else if (col.gameObject.layer == LayerMask.NameToLayer("Line")){
                Debug.Log("Hit Line!");
            }
        }
    }
}
