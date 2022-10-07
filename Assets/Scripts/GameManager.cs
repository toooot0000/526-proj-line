using System;
using BackendApi;
using Core.DisplayArea.Stage;
using Model;
using Tutorials;
using UI;
using UnityEngine;
using Utility;

public class GameManager : MonoBehaviour{
    public StageManager stageManager;
    public TutorialManager tutorialManager;
    
    public static GameManager shared;
    [HideInInspector]
    public bool isAcceptingInput = true;
    public Game game;
    public Guid uuid = Guid.NewGuid();

    private int _currentTurnNum = 0;


    private void Awake(){
        if (shared != null) Destroy(gameObject);
        shared = this;
        InitGame();
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
        stageManager.OnStageLoaded(game.currentStage);
        SwitchToPlayerTurn();
    }

    private void PreInit(){
        // Backend API url
        EventLogger.serverURL = "https://test526.wn.r.appspot.com/";
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
        isAcceptingInput = true;
        if (_currentTurnNum == 2){
            tutorialManager.LoadTutorial("TutorialTurn2");
        }
    }

    private void SwitchToEnemyTurn(){
        isAcceptingInput = false;
        var stageInfo = game.CurrentEnemy.GetCurrentStageAction();
        stageManager.ProcessStageActionInfo(stageInfo);
    }

    public void OnPlayerFinishInput(){
        var currentAction = game.player.GetAttackActionInfo();
        game.player.ClearAllBalls();
        stageManager.ProcessStageActionInfo(currentAction);
    }
}