using System;
using Core.Common;
using Core.DisplayArea.Stage.Player;
using Model;
using TMPro;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{
    public abstract class DamageableView: MonoBehaviour{
        public delegate void DamageableDelegate();
        public event DamageableDelegate OnDie;
        
        public bool isDead = false;

        private IDamageable _model = null;
        public virtual IDamageable Model{
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
                tmp.text = $"{Math.Max(value, 0).ToString()}/{Model.HpUpLimit.ToString()}";
                progressBar.Percentage = (float)value / Model.HpUpLimit * 100f;
                if (_currentHp <= 0) Die();
            }
            get => _currentHp;
        }

        public TextMeshProUGUI tmp;
        public ProgressBar progressBar;
        public PlayerAnimationController animationController;
        public DamageNumberDisplay damageNumberDisplay;
        public StageManager.StageActionInfoWrapper wrappedActionInfo;
        public ArmorDisplayer armorDisplayer;


        public abstract void Attack();

        public abstract void TakeDamage();
        
        public virtual void Die(){
            isDead = true;
            OnDie?.Invoke();
        }
    }
}