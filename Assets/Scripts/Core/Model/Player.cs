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
        
        public Gear[] Add<Gear>(Gear[] gears, Gear item)
        {
            if (gears == null)
            {
                gears = new Gear[]{};
            }

            Gear[] result = new Gear[gears.Length + 1];
            gears.CopyTo(result, 0);
            result[gears.Length] = item;
            return result;
        }

        public Gear[] DeleteAt<Gear>(Gear[] gears, int index)
        {
            Gear[] res = new Gear[gears.Length - 1];
            if( index > 0 )
                Array.Copy(gears, 0, res, 0, index);

            if( index < gears.Length - 1 )
                Array.Copy(gears, index + 1, res, index, gears.Length - index - 1);

            return res;
        }

        public Gear GetItem<Gear>(Gear[] gears, int index, Gear defaultGear)
        {
            if (index < gears.Length)
            {
                return gears[index];
            }
            return defaultGear;
        }

        public Gear[] GetAllItem<Gear>(Gear[] gears)
        {
            Gear[] res = new Gear[gears.Length];
            gears.CopyTo(res, 0);
            return res;
        }
        
    }
}