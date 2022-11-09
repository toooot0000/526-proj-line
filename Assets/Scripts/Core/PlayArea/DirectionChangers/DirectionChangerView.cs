using System;
using Model.Mechanics.PlayableObjects;
using UnityEngine;

namespace Core.PlayArea.DirectionChangers{
    public class DirectionChangerView: PlayableObjectViewBase, ISliceableView, ICircleableView{
        
        public SpriteRenderer spriteRenderer;

        private DirectionChanger _model;
        public DirectionChanger Model{
            set{
                _model = value;
                transform.localScale = new Vector3(value.size, value.size, value.size);
                transform.rotation = Quaternion.Euler(0, 0, Model.targetAngle);
            }
            get => _model;
        }

        public void OnTriggerEnter2D(Collider2D col){
            var movable = col.GetComponent<IMovableView>();
            if (movable == null) return;
            var init = movable.Velocity;
            movable.Velocity = Model.targetDirection.normalized * init.magnitude;
        }

        public void OnSliced(){
            Model.OnSliced().Execute();
            transform.rotation = Quaternion.Euler(0, 0, Model.targetAngle);
        }

        public void OnCircled(){
            Model.OnCircled().Execute();
            transform.rotation = Quaternion.Euler(0, 0, Model.targetAngle);
        }

        private void Start(){
            Model = new DirectionChanger(GameManager.shared.game, 23);
        }
    }
}