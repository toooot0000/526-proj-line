using System;
using Core.Common;
using Core.Model;
using TMPro;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{
    public class DamageableView: MonoBehaviour{

        public delegate void DamageableDelegate();

        public event DamageableDelegate OnDie;
        
        public bool isDead = false;

        private Model.IDamageable _model = null;
        public virtual Model.IDamageable Model{
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
        public StageManager.DamageWrapper damage;


        public void Attack(){
            animationController.Play(PlayerAnimation.Attack);
        }

        public void ProcessDamage(){
            damageNumberDisplay.Number = damage.raw.point;
            CurrentHp -= damage.raw.point;
            if (isDead){
                damage.target.animationController.Play(PlayerAnimation.Die, damage.resolvedCallback);
            } else{
                damage.target.animationController.Play(PlayerAnimation.BeingAttacked, damage.resolvedCallback);
            }
        }
        
        public void SendDamage(){
            damage.target.ProcessDamage();
        }

        public virtual void Die(){
            isDead = true;
            OnDie?.Invoke();
        }
    }
}