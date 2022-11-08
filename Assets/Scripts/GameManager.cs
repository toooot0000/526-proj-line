using System;
using System.Collections;
using System.Collections.Generic;
using BackendApi;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
using Model.Buff;
using Model.Buff.Buffs;
using Tutorial;
using UI;
using UI.Interfaces.SelectStage;
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
    public event Action OnPlayerAttack; // Used by EventLogger

    private void Awake()
    {
        if (shared != null) Destroy(gameObject);
        shared = this;
        PreInit();
        InitGame();
    }
    
    private IEnumerator Start(){
        Application.targetFrameRate = 120;
        UIManager.shared.OnCloseUI += ui => {
            if (ui is not UIShopSystem shop) return;
            _isInShop = false;
        };
        
        UIManager.shared.OnCloseUI += ui => {
            if (ui is not UIResult) return;
            _isInEvent = false;
        };
        UIManager.shared.OpenUI("UIGameStart");
        yield return BeforeGameStart();
    }

    private void Update(){
        if (Input.GetKeyUp(KeyCode.T)){
            game.player.AddBuffLayer<BuffWeak>(1);
        }

        if (Input.GetKeyUp(KeyCode.R)){
            Debug.Log(Buff.BuffsToString(game.player));
        }
        
        // if (Input.GetKeyUp(KeyCode.B)){
        //     UIManager.shared.OpenUI("UIEvent", new Model.Event(game, 0));
        // }
    }

    private void InitGame(){
        game ??= new Game();
    }

    private IEnumerator BeforeGameStart()
    {
        yield return new WaitForEndOfFrame();
        EventLogger.Shared.init(); //should do this after game is initialized   
    }

    public void GameStart(){
        GotoStage((int)CsvLoader.GetConfig("init_stage"));
    }

    public void GameComplete(){
        game.Complete();
        UIManager.shared.OpenUI("UIGameComplete");
    }

    public void GameEnd(){
        game.End();
        UIManager.shared.OpenUI("UIGameEnd");
    }

    public void GameRestart(){
        game.Restart();
        GotoStage((int)CsvLoader.GetConfig("init_stage"));
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

    public void GotoStage(int id){
        game.LoadStage(id);
        StartCoroutine(game.currentStage.type switch{
            StageType.Battle => stageManager.StartBattleStage(),
            StageType.Shop => StartShopStage(),
            StageType.Event => StartEventStage(),
            _ => throw new ArgumentOutOfRangeException()
        });
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


    public void OnPlayerFinishInput(){
        if (game.turn != Game.Turn.Player) return;
        OnPlayerAttack?.Invoke();
        StartCoroutine(stageManager.StartPlayerAction());
    }
}