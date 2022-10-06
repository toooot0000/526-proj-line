using System;
using BackendApi;
using Model;
using Tutorials;
using UnityEngine;
using Utility;

public class GameManager : MonoBehaviour{
    public static GameManager shared;

    public TutorialManager tutorialManager;


    public bool isAcceptingInput = true;

    public Game game;
    public Guid uuid = Guid.NewGuid();


    private void Awake(){
        if (shared != null) Destroy(gameObject);
        shared = this;
        InitGame();
    }

    private void Start(){
        Application.targetFrameRate = 120;
    }

    private void Update(){
        if (Input.GetKeyUp("k")) print("kill the current enemy!\n");
    }

    private void InitGame(){
        PreInit();
        game ??= new Game();
        game.OnTurnChanged += game1 => {
            if (game1.turn == Game.Turn.Player) isAcceptingInput = true;
        };
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
        if (game.currentStage.nextStage == -1)
            Complete();
        else
            game.LoadStage(game.currentStage.nextStage);
    }

    public void Complete(){
        game.Complete();
    }

    public void Restart(){
        game.Restart();
    }

    private void SwitchToPlayerTurn(){
        isAcceptingInput = true;
    }

    private void SwitchToEnemyTurn(){
        isAcceptingInput = false;
    }

    public void OnPlayerFinishInput(){
        var currentAction = game.player.GetAttackAction();
        game.currentStage.ProcessStageAction(currentAction);
    }
}