using System;
using System.Collections;
using BackendApi;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
using Tutorial;
using Tutorial.Tutorials.BasicConcept;
using Tutorial.Tutorials.EnemyIntention;
using Tutorial.Tutorials.Stage1Soft;
using Tutorials;
using UI;
using UI.TurnSignDisplayer;
using UnityEngine;
using Utility;

public class GameManager : MonoBehaviour{
    public static GameManager shared;
    
    public StageManager stageManager;
    public TutorialManager tutorialManager;
    public Game game;
    public Guid uuid = Guid.NewGuid();
    public BallManager ballManager;
    public TurnSignDisplayer turnSignDisplayer;
    public TouchTracker touchTracker;
    public int CurrentTurnNum => game.currentTurnNum;


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
        StartCoroutine(StartBattleStage());
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
        EventLogger.serverURL = "https://test526.wn.r.appspot.com/";
        // EventLogger.serverURL = "http://localhost:8080/";
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
        StartCoroutine(game.turn == Game.Turn.Player ? SwitchToPlayerTurn() : SwitchToEnemyTurn());
    }

    public void GotoStage(int id){
        game.LoadStage(id);
        stageManager.OnStageLoaded(game.currentStage);
        // Temp
        StartCoroutine(StartBattleStage());
    }

    private IEnumerator StartBattleStage(){
        yield return SwitchToPlayerTurn();
    }

    public void Complete(){
        game.Complete();
    }

    public void Restart(){
        game.Restart();
    }

    private IEnumerator SwitchToPlayerTurn(){
        yield return turnSignDisplayer.Show(Game.Turn.Player);
        touchTracker.isAcceptingInput = true;
        ballManager.SpawnBalls();
        if (game.currentStage.id == 0){
            switch (CurrentTurnNum){
                case 1:
                    StartCoroutine(CoroutineUtility.Delayed(1, 
                        () => tutorialManager.LoadTutorial(TutorialBasicConcept.PrefabName)));
                    break;
                case 2:
                    tutorialManager.LoadTutorial(TutorialStage1Soft.PrefabName);
                    break;
            }
        }
    }

    private IEnumerator SwitchToEnemyTurn(){
        var stageInfo = game.CurrentEnemy.GetCurrentStageAction();
        yield return turnSignDisplayer.Show(Game.Turn.Enemy);
        if (game.currentStage.id == 0){
            switch (CurrentTurnNum){
                case 1:
                    tutorialManager.LoadTutorial(TutorialEnemyIntention.PrefabName);
                    break;
            }
        }
        stageManager.ProcessStageActionInfo(stageInfo);
    }

    public void OnPlayerFinishInput(){
        if (game.turn != Game.Turn.Player) return;
        var currentAction = game.player.GetAttackActionInfo();
        game.player.ClearAllBalls();
        stageManager.ProcessStageActionInfo(currentAction);
    }
}