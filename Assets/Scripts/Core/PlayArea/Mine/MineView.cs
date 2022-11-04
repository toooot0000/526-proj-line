using System;
using Core.DisplayArea.Stage;
using Core.PlayArea.TouchTracking;
using Model;
using UnityEngine;
using Utility;
using Utility.Animation;
using Utility.Bezier;

namespace Core.PlayArea.Mine{
    public enum MineState{
        Idle,
        Triggered,
        Removed
    }
    
    public class MineView: MonoBehaviour{
        public MineAnimationController animationController;
        public Model.Obstacles.Mine model;
        public StageManager stageManager;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public float flyTime = 0.2f;
        public TouchTracker tracker;
        public MineState state = MineState.Idle;
        public SpriteRenderer renderer;

        public void Init() {
            state = MineState.Idle;
            renderer.color = Color.white;
            animationController.Play(MineAnimation.Idle);
        }

        private void OnMouseEnter(){
            if (!tracker.isTracing) return;
            if (tracker.LineReachingLimit()) return;
            if (state != MineState.Idle) return;
            state = MineState.Triggered;
            tracker.TryToContinueTracking();
            FlyToLocation(flyTime, stageManager.playerView.transform.position, () => {
                animationController.Play(MineAnimation.Explosion, () => {
                    model.effect.Execute();
                    GameManager.shared.game.playArea.RemovePlayableObject(model);
                    gameObject.SetActive(false);
                });
            });
        }

        public void OnBeingCircled(){
            if (state != MineState.Idle) return;
            state = MineState.Removed;
            animationController.Play(MineAnimation.Disappear, () => {
                GameManager.shared.game.playArea.RemovePlayableObject(model);
                gameObject.SetActive(false);
            });
        }
        
        
        private void FlyToLocation(float seconds, Vector3 targetWorldLocation, Action completion){
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
            targetWorldLocation.z = startWorldLocation.z;
            var lerp = TweenUtility.Lerp(
                seconds,
                null,
                i => {
                    i = curve.Evaluate(i);
                    transform.position = BezierLerp.GetPoint(startWorldLocation, p1, p2, targetWorldLocation, i);
                },
                completion
                );
            StartCoroutine(lerp());
        }
    }
}