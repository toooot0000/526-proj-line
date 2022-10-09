using Model;
using TMPro;

namespace UI.StatusBar{
    public class TurnIndicator : UIComponent{
        private TextMeshProUGUI _textMesh;

        private void Start(){
            _textMesh = GetComponent<TextMeshProUGUI>();
            GameManager.shared.game.OnTurnChanged += UpdateText;
            UpdateText(GameManager.shared.game);
        }
        
        public override void Hide(){
            
        }

        public override void Show(){
            
        }

        private void UpdateText(Game game){
            if (game.turn == Game.Turn.Player)
                _textMesh.text = "Your turn!";
            else
                _textMesh.text = "Enemy's turn!";
        }
    }
}