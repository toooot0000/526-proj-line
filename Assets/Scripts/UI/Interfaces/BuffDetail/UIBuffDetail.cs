using Model.Buff;

namespace UI.Interfaces.BuffDetail{

    public struct UIBuffDetailOption : IUISetUpOptions<UIBuffDetail>{
        public Buff buff;
        public void ApplyOptions(UIBuffDetail uiBase){
            
        }
    }

    public class UIBuffDetail: UIBase{
        
        public override void Open(object arg){
            base.Open(arg);
        }
    }
}