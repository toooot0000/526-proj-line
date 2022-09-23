using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

public class Shade : MonoBehaviour{

    private SpriteRenderer _renderer;

    private void Start(){
        _renderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }


    public void SetActive(bool val){
        if (val){
            var col = _renderer.color;
            gameObject.SetActive(true);
            StartCoroutine(TweenUtility.Lerp(
                    0.2f,
                    () => {
                        _renderer.color = new Color(col.r, col.g, col.b, 0);
                    },
                    i => _renderer.color = new Color(col.r, col.g, col.b, i*0.5f),
                    () => { }
                )()
            );
        } else{
            var col = _renderer.color;
            StartCoroutine(TweenUtility.Lerp(
                    0.2f,
                    () => _renderer.color = new Color(col.r, col.g, col.b, 0.5f),
                    i => _renderer.color = new Color(col.r, col.g, col.b, (1 - i)*0.5f),
                    () => {
                        gameObject.SetActive(false);
                    }
                )()
            );
        }
    }
}