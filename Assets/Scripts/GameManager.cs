using System;
using BackendApi;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Model;
using Tutorials;
using UI;
using UnityEngine;
using Utility;

public class GameManager : MonoBehaviour{
    public StageManager stageManager;
    public TutorialManager tutorialManager;
    
    public static GameManager shared;
    
    public Game game;
    public Guid uuid = Guid.NewGuid();
    public BallManager ballManager;
    public TurnSignDisplayer turnSignDisplayer;

    private int _currentTurnNum = 0;


    private void Awake()
    {
        if (shared != null) Destroy(gameObject);
        shared = this;
        InitGame();
        StartCoroutine(CoroutineUtility.Delayed(1, () =>
            UIManager.shared.OpenUI("UIGameStart")));
        EventLogger.Shared.init();//should do this afeter game is initialized
    }

    private void Start(){
        Application.targetFrameRate = 120;
    }

    private void Update(){
        if (Input.GetKeyUp("e"))
        {
            print("enter the stage");
            GameManager.shared.game.TestOnStageLoaded();
        }

        if (Input.GetKeyUp("s"))
        {
            print("stage success");
            GameManager.shared.game.currentStage.TestOnStageBeaten();
        }
    }

    private void InitGame(){
        PreInit();
        game ??= new Game();
        game.CreatePlayer();
    }

    public void GameStart(){
        game.LoadStage(0);
        stageManager.OnStageLoaded(game.currentStage);
        SwitchToPlayerTurn();
    }

    public void GameComplete(){
        game.Complete();
    }

    public void GameEnd(){
        game.End();
    }

    public void GameRestart(){
        game.Restart();
        game.CreatePlayer();
        game.LoadStage(0);
    }

    private void PreInit(){
        // Backend API url
        // EventLogger.serverURL = "https://test526.wn.r.appspot.com/";
        EventLogger.serverURL = "http://localhost:8080/";
    }

    public void Delayed(int frames, Action modelAction){
        StartCoroutine(CoroutineUtility.Delayed(frames, modelAction));
    }

    public void Delayed(float seconds, Action modelAction){
        StartCoroutine(CoroutineUtility.Delayed(seconds, modelAction));
    }

    /// <summary>
    ///     Kick off the switch turn procedure
    /// </summary>
    public void SwitchTurn(){
        game.SwitchTurn();
        if (game.turn == Game.Turn.Player)
            SwitchToPlayerTurn();
        else
            SwitchToEnemyTurn();
    }

    public void GotoStage(int id){
        _currentTurnNum = 0;
        game.LoadStage(id);
        stageManager.OnStageLoaded(game.currentStage);
        SwitchToPlayerTurn();
    }

    [Obsolete]
    public void GoToNextStage(){
        _currentTurnNum = 0;
        if (game.currentStage.nextStage == -1)
            Complete();
        else{
            game.LoadStage(game.currentStage.nextStage);
            stageManager.OnStageLoaded(game.currentStage);
            SwitchToPlayerTurn();
        }
    }

    public void Complete(){
        game.Complete();
    }

    public void Restart(){
        game.Restart();
    }

    private void SwitchToPlayerTurn(){
        _currentTurnNum++;
        ballManager.SpawnBalls();
        if (game.currentStage.id == 0){
            switch (_currentTurnNum){
                case 1:
                    StartCoroutine(CoroutineUtility.Delayed(1, () => tutorialManager.LoadTutorial("TutorialSliceBall")));
                    break;
                case 2:
                    tutorialManager.LoadTutorial("TutorialTurn2");
                    break;
                case 3:
                    tutorialManager.LoadTutorial("TutorialCharge");
                    break;
            }
        }
    }

    private void SwitchToEnemyTurn(){
        var stageInfo = game.CurrentEnemy.GetCurrentStageAction();
        stageManager.ProcessStageActionInfo(stageInfo);
    }

    public void OnPlayerFinishInput(){
        if (game.turn != Game.Turn.Player) return;
        var currentAction = game.player.GetAttackActionInfo();
        game.player.ClearAllBalls();
        stageManager.ProcessStageActionInfo(currentAction);
    }
}