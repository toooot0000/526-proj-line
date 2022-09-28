using System;
using System.Linq;
using BackendApi;
using Model;
using UnityEngine;
using Utility;
using Utility.Loader;

public class GameManager : MonoBehaviour{
    public static GameManager shared;

    public Game game;
    public bool isAcceptingInput = true;

    private void Awake()
    {
        if (shared != null){
            Destroy(this.gameObject);
        }
        shared = this;
        InitGame();
    }

    private void Start(){
        Application.targetFrameRate = 120;
    }

    private void Update(){ }


    private void InitGame() {
        PreInit();
        game ??= new();
        game.OnTurnChanged += game1 => {
            if (game1.turn == Game.Turn.Player) isAcceptingInput = true;
        };
    }

    private void PreInit() {
        // Backend API url
        EventLogger.serverURL = "https://test526.wn.r.appspot.com/";
    }

    public void LogTable(string tableName){
        var table = CsvLoader.Load(tableName);
        foreach (var pair in table){
            Debug.Log(pair.Key.ToString());
            var msg = pair.Value.Aggregate("", (current, content) => current + $"{content.Key} = {content.Value}, ");
            Debug.Log(msg);
        }
    }

    public void Delayed(int frames, Action modelAction) {
        StartCoroutine(CoroutineUtility.Delayed(frames, modelAction));
    }

    public void Delayed(float seconds, Action modelAction) {
        StartCoroutine(CoroutineUtility.Delayed(seconds, modelAction));
    }
}
