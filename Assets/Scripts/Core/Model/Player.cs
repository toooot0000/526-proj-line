using System;
using System.Collections.Generic;
using System.Linq;
using BackendApi;
using UnityEngine;

namespace Core.Model{
    [Serializable]
    public class Player: GameModel, IDamageable{

        public int hpUpLimit;
        public int currentHp;
        public Gear[] gears;
        public int gearUpLimit;
        public int energy;

        public List<Ball> hitBalls = new();
        public List<Ball> circledBalls = new();

        public event ModelEvent OnHitBall;
        public event ModelEvent OnCircledBall;

        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;
        
        public void TakeDamage(Damage damage){
            currentHp -= damage.point;
            Debug.Log($"Player take damage(point {damage.point}, current HP: {currentHp})");
            OnBeingAttacked?.Invoke(currentGame, this);
        }


        public Player(GameModel parent) : base(parent){ }

        public void AddHitBall(Ball ball){
            hitBalls.Add(ball);
            OnHitBall?.Invoke(currentGame, ball);
        }

        public void AddCircledBall(Ball ball){
            circledBalls.Add(ball);
            OnCircledBall?.Invoke(currentGame, ball);
        }

        public int GetTotalPoint(){
            return hitBalls.Sum(ball => ball.point) + circledBalls.Sum(ball => ball.point * 2);
        }
        public void Attack(){
            var dmg = new Damage(){
                point = GetTotalPoint(),
                type = Damage.Type.Physics,
                target = currentGame.CurEnemy
            };
            hitBalls.Clear();
            circledBalls.Clear();
            Debug.Log($"Player attack! with point {dmg.point.ToString()}");
            dmg.target.TakeDamage(dmg);
            OnAttack?.Invoke(currentGame, this);
        }
    }
}