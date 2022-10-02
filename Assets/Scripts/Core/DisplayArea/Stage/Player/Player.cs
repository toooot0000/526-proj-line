using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class Player: DamageableView{

        private void Start(){
            Model = GameManager.shared.game.player;
        }

        public override void Die(){
            base.Die();
            StartCoroutine(CoroutineUtility.Delayed(0.5f, () => {
                UIManager.shared.OpenUI("UIGameEnd");
            }));
        }

        public override void Attack(){
            // Play Attack Animation/defends animation/special attack animation;
        }
    }
}