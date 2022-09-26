using System;
using Core.Model;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    public class TurnIndicator: MonoBehaviour{
        private TextMeshProUGUI _textMesh;

        private void Start(){
            _textMesh = GetComponent<TextMeshProUGUI>();
            GameManager.shared.game.OnTurnChanged += UpdateText;
            UpdateText(GameManager.shared.game);
        }

        private void UpdateText(Game game){
            if (game.turn == Game.Turn.Player){
                _textMesh.text = "Your turn!";
            } else{
                _textMesh.text = "Enemy's turn!";
            }
        }
    }
}