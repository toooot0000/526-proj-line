using Core.Common;
using Core.Model;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea.Stage{
    public class DamageableView: MonoBehaviour{
        

        private Model.IDamageable _model = null;
        public Model.IDamageable Model{
            set{
                _model = value;
                CurrentHp = _model.HpUpLimit;
            }
            get => _model;
        }

        private int _currentHp = 0;
        public int CurrentHp{
            set{
                _currentHp = value;
                tmp.text = $"{value.ToString()}/{Model.HpUpLimit.ToString()}";
                progressBar.Percentage = (float)value / Model.HpUpLimit * 100f;
            }
            get => _currentHp;
        }
        
        public TextMeshProUGUI tmp;
        public ProgressBar progressBar;
        public PlayerAnimationController animationController;
        public DamageNumberDisplay damageNumberDisplay;
        public StageManager.DamageWrapper damage;
        
        
        public void SendDamage(){
            damage.target.CurrentHp -= damage.raw.point;
            damage.target.animationController.PlayAnimation(PlayerAnimationController.PlayerAnimation.BeingAttacked);
            damage.target.damageNumberDisplay.Number = damage.raw.point;
        }
    }
}