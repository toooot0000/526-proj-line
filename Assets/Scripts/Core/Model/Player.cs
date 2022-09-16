using System;
using System.Collections.Generic;
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
            var ret = 0;
            foreach (var ball in hitBalls){
                ret += ball.point;
            }

            foreach (var ball in circledBalls){
                ret += ball.point * 2;
            }
            return ret;
        }

    }
}