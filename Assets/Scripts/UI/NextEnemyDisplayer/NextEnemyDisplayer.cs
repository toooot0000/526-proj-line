using UI.Common.SimpleAnimation;

namespace UI.NextEnemyDisplayer{
    public class NextEnemyDisplayer: UIComponent{
        public EdgeHider edgeHider;

        private void Start(){
            UIManager.shared.RegisterComponent(this);
            
        }

        public override void Hide(){
            edgeHider.Hide();
        }

        public override void Show(){
            edgeHider.Show();
        }
    }
}