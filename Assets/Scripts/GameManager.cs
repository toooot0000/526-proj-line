using System;
using System.Collections;
using BackendApi;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
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
        if (Input.GetKeyUp("k")){
            UIManager.shared.OpenUI("UIShopSystem", (object)(new Gear[]{
                new Gear(game, -1),
                new Gear(game, 1),
                new Gear(game, 2),
                new Gear(game, 3),
                new Gear(game, 4),
                new Gear(game, 5)
            }));
        };
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
        _currentTurnNum = 0;
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
        _currentTurnNum++;
        yield return turnSignDisplayer.Show(Game.Turn.Player);
        touchTracker.isAcceptingInput = true;
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

    private IEnumerator SwitchToEnemyTurn(){
        var stageInfo = game.CurrentEnemy.GetCurrentStageAction();
        yield return turnSignDisplayer.Show(Game.Turn.Enemy);
        stageManager.ProcessStageActionInfo(stageInfo);
    }

    public void OnPlayerFinishInput(){
        if (game.turn != Game.Turn.Player) return;
        var currentAction = game.player.GetAttackActionInfo();
        game.player.ClearAllBalls();
        stageManager.ProcessStageActionInfo(currentAction);
    }
}