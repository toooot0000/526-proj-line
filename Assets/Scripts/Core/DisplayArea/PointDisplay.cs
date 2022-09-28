using System;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.DisplayArea{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PointDisplay: MonoBehaviour{
        private TextMeshProUGUI _text;
        private Game _game;
        private void Start(){
            _text = GetComponent<TextMeshProUGUI>();
            _game = GameManager.shared.game;
            _game.player.OnHitBall += (game, model) => UpdateNumber();
            _game.player.OnCircledBall += (game, model) => UpdateNumber();
            _game.player.OnAttack += (game, model) => UpdateNumber();
            UpdateNumber();
        }

        private void UpdateNumber(){
            _text.text = $"{_game.player.GetTotalPoint().ToString()}";
        }
    }
}