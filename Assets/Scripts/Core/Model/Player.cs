using System;
using System.Collections.Generic;
using System.Linq;
using BackendApi;

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
            OnAttack?.Invoke(currentGame, this);
        }
    }
}