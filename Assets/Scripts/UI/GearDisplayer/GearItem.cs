using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Loader;

namespace UI.GearDisplayer{
    public class GearItem : MonoBehaviour{

        public Image icon;
        private Gear _model;
        
        public Gear Model{
            set{
                _model = value;
                UpdateContent();
            }
            get => _model;
        }

        public void ShowGearInfo(){
            UIManager.shared.OpenUI("UIGearInfo", _model);
        }
        
        public void UpdateContent(){
            icon.sprite = Resources.Load<Sprite>(Model.imgPath);
        }
    }
}