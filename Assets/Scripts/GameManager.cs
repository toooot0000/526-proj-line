using BackendApi;
using Core.Model;
using UnityEngine;
using Utility.Loader;

public class GameManager : MonoBehaviour{
    public static GameManager shared;

    public Game game;

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


    private void InitGame() {
        PreInit();
        game ??= new Game();
        var balls = CsvLoader.Load("Configs/balls");
        Debug.Log(balls.ToString());
    }

    private void PreInit() {
        // Backend API url
        EventLogger.serverURL = "https://test526.wn.r.appspot.com/";
    }
}
