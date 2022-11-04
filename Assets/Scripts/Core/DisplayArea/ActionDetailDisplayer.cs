using Model;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    public class ActionDetailDisplayer : MonoBehaviour{
        private Game _game;
        public TextMeshProUGUI text;

        private void Start(){
            _game = GameManager.shared.game;
            _game.player.OnHitBall += (game, model) => UpdateNumber();
            _game.player.OnCircledBall += (game, model) => UpdateNumber();
            _game.player.OnAttack += (game, model) => UpdateNumber();
            UpdateNumber();
        }

        private void UpdateNumber(){
            var info = _game.player.GetAction();
            info.ExecuteSpecials();
            var attStr = info.damage.initPoint > 0 ? $"Att: {info.damage.initPoint}" : "";
            var defStr = info.defend > 0 ? $"Def: {info.defend}" : "";
            text.text = $"{attStr} {defStr}";
        }
    }
}