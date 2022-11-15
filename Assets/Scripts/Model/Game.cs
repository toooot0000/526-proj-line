using System.Collections.Generic;
using System.Linq;
using Model.Mechanics;
using Model.Mechanics.PlayableObjects;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Model{

    public class Game : GameModel{
        public enum Turn{
            Player,
            Enemy
        }

        public readonly Stage currentStage;
        public Player player;
        public Turn turn = Turn.Player;
        public PlayArea playArea;


        public bool IsLastStage => currentStage.IsLast;
        public Enemy CurrentEnemy => currentStage?.CurrentEnemy;
        
        public int currentTurnNum = 0;
        public event SimpleModelEvent OnTurnChanged;
        public event SimpleModelEvent OnGameEnd;
        public event SimpleModelEvent OnGameComplete;
        public event SimpleModelEvent OnStageLoaded;
        public event SimpleModelEvent OnGameRestart;
        public event SimpleModelEvent OnPlayerInit;
        
        public Game(GameModel parent = null) : base(parent){
            currentGame = this;
            currentStage = new Stage(this);
            CreatePlayer();
        }
        
        public Ball[] GetAllBalls(){
            var ret =  player.GetAllBallsWithoutGridPosition();
            var emptyPositions = playArea.GetEmptyGridPositions().ToArray();
            foreach (var ball in ret){
                ball.InitGridRectInt = new RectInt(){
                    position = emptyPositions[Random.Range(0, emptyPositions.Length)],
                    size = new Vector2Int(1, 1),
                };
            }
            return ret;
        }

        public void SwitchTurn(){
            turn = turn == Turn.Player ? Turn.Enemy : Turn.Player;
            if (turn == Turn.Player) currentTurnNum++;
            OnTurnChanged?.Invoke(this);
        }

        public void LoadStage(int id){
            currentStage.LoadFromConfig(id);
            currentTurnNum = 1;
            OnStageLoaded?.Invoke(this);
            turn = Turn.Player;
            OnTurnChanged?.Invoke(this);
        }

        private void CreatePlayer(){
            player = new Player(this);
            playArea = new PlayArea(this);
            OnPlayerInit?.Invoke(this);
        }

        public void End(){
            OnGameEnd?.Invoke(this);
        }

        public void Complete(){
            OnGameComplete?.Invoke(this);
        }

        public void Restart(){
            Reset();
            OnGameRestart?.Invoke(this);
        }

        public void Reset(){
            CreatePlayer();
        }
    }
}