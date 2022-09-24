using System;
using System.Collections.Generic;

namespace Core.Model{
    [Serializable]
    public class Ball: GameModel{
        public enum Type{
            Physics,
            Magic,
            Defend,
        }

        public int id;
        public String desc;
        public Type type;
        public int point;
        public float size;
        public float speed;
        public float charge;
        public float combo;
        
        public Ball(GameModel parent) : base(parent){ }
        
        public float chargeEffect(Player player, List<Ball> circledBalls)
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