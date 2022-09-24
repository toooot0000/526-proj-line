﻿using System;
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
        public float armor;

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
            var dmg = new Damage(){
                point = GetTotalPoint(),
                type = Damage.Type.Physics,
                target = currentGame.CurEnemy
            };
            dmg.target.TakeDamage(dmg);
            OnAttack?.Invoke(currentGame, this);
        }
        
        public Gear[] Add<Gear>(Gear gear)
        {
            Gear[] result = new Gear[gears.Length + 1];
            gears.CopyTo(result, 0);
            result[gears.Length] = gear;
            return result;
        }

        public Gear[] DeleteAt<Gear>(int index)
        {
            Gear[] res = new Gear[gears.Length-1];
            if( index > 0 )
                Array.Copy(gears, 0, res, 0, index);

            if( index < gears.Length-1 )
                Array.Copy(gears, index + 1, res, index, gears.Length - index - 1);

            return res;
        }

        public Gear[] GetAllGears()
        {
            Gear[] res = new Gear[gears.Length];
            gears.CopyTo(res, 0);
            return res;
        }

        public float chargeEffect(List<Ball> circledBalls)
        {
            float points = 0;
            if (circledBalls.Count == 0)
            {
                return points;
            }
            else
            {
                Dictionary<Ball, int> res = new Dictionary<Ball, int>();
                for (int i = 0; i < circledBalls.Count; i++)
                {
                    if (res.ContainsKey(circledBalls[i]))
                    {
                        res[circledBalls[i]] += 1;
                    }
                    else
                    {
                        res.Add(circledBalls[i],1);
                    }
                }

                foreach (Ball circleball in res.Keys)
                {
                    points += circleball.charge * circleball.point * res[circleball];
                }
            }
            return points;
        }
        
        public float comboEffect(Player player,List<Ball> hitBalls, List<Ball> circledBalls){
            float points = 0;
            if(hitBalls.Count == 0) 
            {
                return points;
            }
            else
            {
                Dictionary<Ball, int> record = new Dictionary<Ball, int>();
                for( int j = 0; j < hitBalls.Count; j++)
                {
                    if(record.ContainsKey(hitBalls[j])){
                        record[hitBalls[j]] += 1;
                    }else{
                        record.Add(hitBalls[j], 1);
                    }
                }
                foreach(Ball hitBall in record.Keys){
                    if(record[hitBall] >= 2){
                        if (hitBall.type != Ball.Type.Defend)
                        {
                            points += hitBall.combo * hitBall.point * record[hitBall];
                        }
                        else
                        {
                            points += hitBall.combo * hitBall.point * record[hitBall];
                            player.armor += points;
                        }
                    }
                }
                return points;
            }

        }
    }
}