using System;
using System.Collections;
using Model;
using Tutorial;
using Tutorial.Tutorials.TurnSign;
using Tutorials;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.TurnSignDisplayer{
    public class TurnSignDisplayer: MonoBehaviour, ITutorialControllable{
        public Sprite playerTurn;
        public Sprite enemyTurn;
        public Image signImage;
        public Image shade;
        public Color shadeColor = new Color(0, 0, 0, 127);
        public float animationTime = 0.5f;
        public float displayTime = 1f;
        public RectTransform startPosition;
        public RectTransform displayPosition;
        public RectTransform endPosition;

        private bool _isInTutorial = false;
        private bool _isPaused = false;

        public IEnumerator Show(Game.Turn turn){
            switch (turn){
                case Game.Turn.Player:
                    signImage.sprite = playerTurn;
                    break;
                case Game.Turn.Enemy:
                    signImage.sprite = enemyTurn;
                    break;
            }
            
            StartCoroutine(ShadeFadeIn());
            StartCoroutine(SprIn());
            yield return new WaitWhile(() => _isPaused);
            yield return new WaitForSeconds(animationTime + displayTime);
            /* Turn Sign Tutorial Begin */
            if (turn == Game.Turn.Enemy && GameManager.shared.game.currentStage.id == 0 &&
                GameManager.shared.CurrentTurnNum == 1){
                GameManager.shared.tutorialManager.LoadTutorial(TutorialTurnSign.PrefabName);
            }
            /* Turn Sign Tutorial End */
            yield return new WaitWhile(() => _isPaused);
            StartCoroutine(ShadeFadeOut());
            StartCoroutine(SprOut());
            yield return new WaitForSeconds(animationTime);
            yield return new WaitWhile(() => _isPaused);
        }

        private IEnumerator SprIn(){
            yield return TweenUtility.Move(animationTime, signImage.transform,
                startPosition.position, displayPosition.position);
        }

        private IEnumerator SprOut(){
            yield return TweenUtility.Move(animationTime, signImage.transform,
                displayPosition.position, endPosition.position);
        }

        private IEnumerator ShadeFadeIn(){
            yield return TweenUtility.Fade(0.5f*animationTime, shade, new Color(0, 0, 0, 0), shadeColor);
        }

        private IEnumerator ShadeFadeOut(){
            yield return TweenUtility.Fade(0.5f*animationTime, shade, shadeColor, new Color(0, 0, 0, 0));
        }

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
        }

        public void TutorSetPause(bool val){
            if (!_isInTutorial) return;
            _isPaused = val;
        }
    }
}