using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using LineUtility = Utility.LineUtility;

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
        // var v1 = new Vector2(0, 0);
        // var v2 = new Vector2(10, 10);
        // var v3 = new Vector2(-10, -1);
        // var v4 = new Vector2(28, 0);
        // var detector = new CircleDetector();
        // detector.AddPoint(new Vector2(0, 0));
        // detector.AddPoint(new Vector2(0, 1));
        // detector.AddPoint(new Vector2(1, 1));
        // detector.AddPoint(new Vector2(1, 0.2f));
        // detector.AddPoint(new Vector2(-1, 0.2f));
        // var circle = detector.DetectCircle();
        // Debug.Log(LineUtility.GetIntersectPoint(v1, v2, v3, v4).ToString());

    }

    private void Start(){
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
