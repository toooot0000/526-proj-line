using System;
using System.Collections;
using System.Collections.Generic;
using BackendApi;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
using Tutorial;
using Tutorial.Tutorials.BasicConcept;
using Tutorial.Tutorials.Charge;
using Tutorial.Tutorials.Combo;
using Tutorial.Tutorials.EnemyIntention;
using Tutorial.Tutorials.Stage1Soft;
using UI;
using UI.Interfaces.ShopSystem;
using UI.Interfaces.SpecialEvent;
using UI.TurnSignDisplayer;
using UnityEngine;
using Utility;
using Utility.Loader;

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

    private bool _isInShop = false;
    private bool _isInEvent = false;


    private void Awake()
    {
        if (shared != null) Destroy(gameObject);
        shared = this;
        InitGame();
        StartCoroutine(CoroutineUtility.Delayed(1, () =>
            UIManager.shared.OpenUI("UIGameStart")));
    }

    private void Start(){
        Application.targetFrameRate = 120;
        
        
        UIManager.shared.OnCloseUI += ui => {
            if (ui is not UIShopSystem shop) return;
            _isInShop = false;
        };
        
        UIManager.shared.OnCloseUI += ui => {
            if (ui is not UIResult) return;
            _isInEvent = false;
        };
    }

    private void Update(){
        if (Input.GetKeyUp(KeyCode.T)){
            UIManager.shared.OpenUI("UIShopSystem");
        }
        
        // if (Input.GetKeyUp(KeyCode.B)){
        //     UIManager.shared.OpenUI("UIEvent", new Model.Event(game, 0));
        // }
    }

    private void InitGame(){
        PreInit();
        game ??= new Game();
        game.CreatePlayer();
    }

    public void GameStart(){
        game.LoadStage((int)CsvLoader.GetConfig("init_stage"));
        EventLogger.Shared.init();//should do this after game is initialized
        stageManager.PresentStage(game.currentStage);
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
        game.LoadStage((int)CsvLoader.GetConfig("init_stage"));
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
        StartCoroutine(game.currentStage.type switch{
            StageType.Battle => StartBattleStage(),
            StageType.Shop => StartShopStage(),
            StageType.Event => StartEventStage()
        });

    }

    private IEnumerator StartBattleStage(){
        stageManager.PresentStage(game.currentStage);
        yield return SwitchToPlayerTurn();
    }

    private IEnumerator StartShopStage(){
        _isInShop = true;
        UIManager.shared.OpenUI("UIShopSystem");
        yield return new WaitWhile(() => _isInShop);
        UIManager.shared.OpenUI("UISelectStage", game.currentStage.nextStageChoice);
    }

    private IEnumerator StartEventStage(){
        _isInEvent = true;
        UIManager.shared.OpenUI("UIEvent", game.currentStage.CurrentEvent);
        yield return new WaitWhile(() => _isInEvent);
        UIManager.shared.OpenUI("UISelectStage", game.currentStage.nextStageChoice);
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
                        () => tutorialManager.LoadTutorial<TutorialBasicConcept>()));
                    break;
                case 2:
                    tutorialManager.LoadTutorial<TutorialStage1Soft>();
                    break;
            }
        } else if (game.currentStage.id == 1){
            switch (CurrentTurnNum){
                case 1:
                    tutorialManager.LoadTutorial<TutorialCombo>();
                    break;
            }

            if (game.currentStage.CurrentEnemy.id == -3){
                tutorialManager.LoadTutorial<TutorialCharge>();
            }
        }
    }

    private IEnumerator SwitchToEnemyTurn(){
        var stageInfo = game.CurrentEnemy.GetCurrentStageAction();
        yield return turnSignDisplayer.Show(Game.Turn.Enemy);
        if (game.currentStage.id == 0){
            switch (CurrentTurnNum){
                case 1:
                    tutorialManager.LoadTutorial<TutorialEnemyIntention>();
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