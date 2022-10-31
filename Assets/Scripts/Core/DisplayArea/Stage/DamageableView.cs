using System;
using Core.Common;
using Core.DisplayArea.Stage.Player;
using Model;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea.Stage{
    public abstract class DamageableView : MonoBehaviour{
        public delegate void DamageableDelegate();

        public bool isDead;

        public TextMeshProUGUI tmp;
        public ProgressBar progressBar;
        public PlayerAnimationController animationController;
        public DamageNumberDisplay damageNumberDisplay;
        public ArmorDisplayer armorDisplayer;

        private int _currentHp;

        private Damageable _model;
        // public StageManager.StageActionInfoWrapper wrappedActionInfo;
        public StageActionBase stageAction;

        public virtual Damageable Model{
            set{
                _model = value;
                CurrentHp = _model.HpUpLimit;
            }
            get => _model;
        }

        public int CurrentHp{
            set{
                _currentHp = value;
                tmp.text = $"{Math.Max(value, 0).ToString()}/{Model.HpUpLimit.ToString()}";
                progressBar.Percentage = (float)value / Model.HpUpLimit * 100f;
                if (value <= 0) isDead = true;
            }
            get => _currentHp;
        }

        public void SyncHp(){
            CurrentHp = Model.CurrentHp;
        }
    }
}