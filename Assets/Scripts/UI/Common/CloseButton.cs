using UI.Interfaces;
using UnityEngine;

namespace UI.Common{
    public class CloseButton: MonoBehaviour{
        public UIBase relatedUI;

        public void OnClick(){
            relatedUI.Close();
        }
    }
}