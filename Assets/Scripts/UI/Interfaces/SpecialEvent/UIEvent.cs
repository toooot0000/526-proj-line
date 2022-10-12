using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Event = Model.Event;
using TMPro;
using UI;
using UI.Interfaces;
using UnityEngine.UIElements;
using Utility;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class UIEvent : UIBase
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField]Sprite defaultAnswerSprite;
    [SerializeField]Sprite selectedAnswerSprite;
    [SerializeField] int eventIndex = 4;
    private CanvasGroup _canvasGroup;
    private bool _inAnimation;

    private void Awake()
    {
        eventIndex = Random.Range(0, 4);
        //eventIndex = 3;
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        
        Event currentEvent = new Event(eventIndex);
        questionText.text = currentEvent.getQuestion();
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentEvent.getAnswer(i);
        }
        
        Debug.Log(GameManager.shared.game.player.gears.Count);

    }
    
    public override void Open(object nextStageChoice) {
        base.Open(nextStageChoice);
        _inAnimation = true;
        var coroutine = TweenUtility.Lerp(0.2f,
            () => _canvasGroup.alpha = 0,
            i => _canvasGroup.alpha = i,
            () => _inAnimation = false
        );
        StartCoroutine(coroutine());
    }

    public override void Close() {
        _inAnimation = true;
        var coroutine = TweenUtility.Lerp(0.2f,
            () => _canvasGroup.alpha = 1,
            i => _canvasGroup.alpha = 1 - i,
            () => {
                _inAnimation = false;
                base.Close();
                Destroy(gameObject);
            });
        StartCoroutine(coroutine());
    } 

    public void OnAnswerSelected(int index)
    {
        Debug.Log("pressed");
        Event e = new Event(eventIndex);
        questionText.text = index.ToString();
        e.explainArgs(index);
        Image buttonImage = answerButtons[index].GetComponent<Image>();
        Debug.Log(GameManager.shared.game.player.gears.Count);
        Close();
        
    }
    
    
}


