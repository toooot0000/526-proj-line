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
        public BuffDisplayer.BuffDisplayer buffDisplayer;

        private int _currentHp;

        private Damageable _model;
        public StageActionBase stageAction;

        public virtual Damageable Model{
            set{
                if (value == null) return;
                if (_model != null) UnbindEvents(_model);
                _model = value;
                CurrentHp = _model.HpUpLimit;
                armorDisplayer.Number = value.Armor;
                BindEvents(_model);
                buffDisplayer.Target = value;
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

        private void UnbindEvents(Damageable damageable){
            damageable.OnHpChanged -= SyncHp;
            damageable.OnArmorChanged -= SyncArmor;
            damageable.OnTakeDamage -= OnTakeDamage;
        }

        private void BindEvents(Damageable damageable){
            damageable.OnHpChanged += SyncHp;
            damageable.OnArmorChanged += SyncArmor;
            damageable.OnTakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(Game game, Damage damage){
            damageNumberDisplay.Number = damage.lifeDeductionPoint;
        }

        private void SyncHp(Game game, Damageable damageable){
            CurrentHp = damageable.CurrentHp;
        }

        private void SyncArmor(Game game, Damageable damageable){
            armorDisplayer.Number = damageable.Armor;
        }

    }
}