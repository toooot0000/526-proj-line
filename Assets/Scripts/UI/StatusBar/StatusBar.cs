using System;
using UI.Common.SimpleAnimation;

namespace UI.StatusBar{
    public class StatusBar: UIComponent{
        private EdgeHider _edgeHider;
        
        private void Start(){
            UIManager.shared.RegisterComponent(this);
            _edgeHider = GetComponent<EdgeHider>();
        }
        
        public override void Hide(){
            _edgeHider.Hide();
        }

        public override void Show(){
            _edgeHider.Show();
        }
    }
}