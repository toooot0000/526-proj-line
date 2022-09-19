using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Model;
using UnityEngine;
using LineUtility = Utility.LineUtility;

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


    private void InitGame(){
        if (game == null){
            game = new Game();
        }
    }
}
