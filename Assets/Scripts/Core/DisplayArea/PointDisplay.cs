using Model;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PointDisplay : MonoBehaviour{
        private Game _game;
        private TextMeshProUGUI _text;

        private void Start(){
            _text = GetComponent<TextMeshProUGUI>();
            _game = GameManager.shared.game;
            _game.player.OnHitBall += (game, model) => UpdateNumber();
            _game.player.OnCircledBall += (game, model) => UpdateNumber();
            _game.player.OnAttack += (game, model) => UpdateNumber();
            UpdateNumber();
        }

        private void UpdateNumber(){
            _text.text = $"Current Energy: {_game.player.GetTotalAttackPoint().ToString()}";
        }
    }
}