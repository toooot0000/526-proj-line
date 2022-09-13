﻿using System;

namespace Core.Model{
    [Serializable]
    public class Enemy: GameModel, IDamageable{
        public int hpUpLimit = 1000;
        public int currentHp;
        public int attack = 1;

        public Enemy(GameModel parent) : base(parent){
            currentHp = hpUpLimit;
        }
        public void TakeDamage(Damage damage){
            currentHp -= damage.point;
        }
    }
}