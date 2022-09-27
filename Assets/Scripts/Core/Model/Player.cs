using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utility.Loader;
using BackendApi;
using Unity.VisualScripting;

namespace Core.Model{
    [Serializable]
    public class Player: GameModel, IDamageable{

        public int hpUpLimit;
        public int currentHp;
        public List<Gear> gears;
        public int gearUpLimit;
        public int energy;
        public float armor;

        public List<Ball> hitBalls = new();
        public List<Ball> circledBalls = new();
        
        public DataTable gearTable;
        public DataTable artifactTable;

        public event ModelEvent OnHitBall;
        public event ModelEvent OnCircledBall;

        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;
        
        public void TakeDamage(Damage damage){
            currentHp -= damage.point;
        }

        public Player(GameModel parent) : base(parent){ }
        
        public Player(GameModel parent, int hpUpLimit, int energy, float armor) : base(parent){
            this.hpUpLimit = 100;
            currentHp = hpUpLimit;
            gears = new List<Gear>();
            gearUpLimit = 3;
            this.energy = energy;
            this.armor = 0;
            gearTable = LoadData("gears.csv");
            artifactTable = LoadData("artifacts.csv");
        }

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

        public float ChargeEffect()
        {
            float points = 0;
            if (circledBalls.Count == 0) {
                return points;
            }
            else {
                Dictionary<Ball, int> res = new Dictionary<Ball, int>();
                for (int i = 0; i < circledBalls.Count; i++) {
                    if (res.ContainsKey(circledBalls[i])) {
                        res[circledBalls[i]] += 1;
                    }
                    else {
                        res.Add(circledBalls[i],1);
                    }
                }

                foreach (Ball circled in res.Keys) {
                    points += circled.charge * circled.point * res[circled];
                }
            }
            return points;
        }
        
        public float ComboEffect(){
            float points = 0;
            if(hitBalls.Count == 0) {
                return points;
            }
            else {
                Dictionary<Ball, int> record = new Dictionary<Ball, int>();
                for( int j = 0; j < hitBalls.Count; j++) {
                    if(record.ContainsKey(hitBalls[j])){
                        record[hitBalls[j]] += 1;
                    }else{
                        record.Add(hitBalls[j], 1);
                    }
                }
                foreach(Ball hitBall in record.Keys){
                    if(record[hitBall] >= 2){
                        if (hitBall.type != Ball.Type.Defend) {
                            points += hitBall.combo * hitBall.point * record[hitBall];
                        }
                        else {
                            points += hitBall.combo * hitBall.point * record[hitBall];
                            armor += points;
                        }
                    }
                }
                return points;
            }

        }

        public void GetGear(int id)
        {
            if (gearTable.Rows.Count > id)
            {
                DataRow row = gearTable.Rows[id];
                Gear gear = new Gear(this, row);
                gears.Add(gear);
            }
        }

        public DataTable LoadData(string filename)
        {
            DataTable dt = new DataTable();
            dt = CsvLoader.LoadCsv(filename);
            return dt;
        }


        // public Dictionary<int, Dictionary<string, object>> LoadData(string filename)
        // {
        //     Dictionary<int, Dictionary<string, object>> data = CsvLoader.Load(filename);
        //     foreach (int id in data.Keys)
        //     {
        //         foreach (string key in data[id].Keys)
        //         {
        //             Console.Out.WriteLine("id: " + id + " key: " + key + " value: " + data[id][key]);
        //         }
        //     }
        //     return data;
        // }
        
        
        
        
    }
}