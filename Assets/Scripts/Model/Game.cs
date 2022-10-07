﻿using System.Collections.Generic;
using UnityEngine;

namespace Model{
    public delegate void ModelEvent(Game game, GameModel model);

    public delegate void SimpleModelEvent(Game game);

    public class Game : GameModel{
        public enum Turn{
            Player,
            Enemy
        }

        public Stage currentStage;
        public Player player;
        public Turn turn = Turn.Player;

        public Game(GameModel parent = null) : base(parent){
            currentGame = this;
        }

        public bool HasNextStage => currentStage?.nextStage != -1;
        public Enemy CurrentEnemy => currentStage?.CurrentEnemy;
        public event SimpleModelEvent OnTurnChanged;
        public event SimpleModelEvent OnGameEnd;
        public event SimpleModelEvent OnGameComplete;
        public event SimpleModelEvent OnStageLoaded;
        public event SimpleModelEvent OnGameRestart;
        public event SimpleModelEvent OnPlayerInit;

        public List<Ball> GetAllSkillBalls(){
            var ret = new List<Ball>();
            foreach (var item in player.CurrentGears)
                for (var i = 0; i < item.ballNum; i++)
                    ret.Add(item.ball);
            return ret;
        }

        public void SwitchTurn(){
            turn = turn == Turn.Player ? Turn.Enemy : Turn.Player;
            OnTurnChanged?.Invoke(this);
        }

        public void LoadStage(int id){
            currentStage = new Stage(this, id);
            OnStageLoaded?.Invoke(this);
            turn = Turn.Player;
            OnTurnChanged?.Invoke(this);
        }

        public void CreatePlayer(){
            player = new Player(this);
            OnPlayerInit?.Invoke(this);
        }

        public void End(){
            OnGameEnd?.Invoke(this);
        }

        public void Complete(){
            OnGameComplete?.Invoke(this);
        }

        public void Restart(){
            OnGameRestart?.Invoke(this);
        }
    }
}