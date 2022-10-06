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

        private IDamageable _model;
        public StageManager.StageActionInfoWrapper wrappedActionInfo;

        public virtual IDamageable Model{
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
                if (_currentHp <= 0) Die();
            }
            get => _currentHp;
        }

        public event DamageableDelegate OnDie;


        public abstract void Attack();

        public abstract void TakeDamage();

        public virtual void Die(){
            isDead = true;
            OnDie?.Invoke();
        }
    }
}