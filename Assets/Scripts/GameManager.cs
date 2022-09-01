using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    private static GameManager _shared = null;

    public static GameManager Shared => _shared;

    // Start is called before the first frame update
    private void Awake()
    {
        if (_shared != null){
            Destroy(this.gameObject);
        }
        _shared = this;
    }

    private void Start(){
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
