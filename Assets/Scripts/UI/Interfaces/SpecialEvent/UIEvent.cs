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
    public TextMeshProUGUI questionText;
    public  GameObject[] answerButtons;
    public Sprite defaultAnswerSprite;
    public Sprite selectedAnswerSprite;
    public  int eventIndex = 4;
    public Event eventModel;
    private CanvasGroup _canvasGroup;
    private bool _inAnimation;

    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;

    }
    
    public override void Open(object model){
        eventModel = (Event)model;
        base.Open(model);
        _inAnimation = true;

        questionText.text = eventModel.desc;
        UpdateButton(eventModel);

        var coroutine = TweenUtility.Lerp(0.2f,
            () => _canvasGroup.alpha = 0,
            i => _canvasGroup.alpha = i,
            () => _inAnimation = false
        );
        StartCoroutine(coroutine());
    }
    
    /// <summary>
    /// 调整按钮显示，包括但不限于更新按钮文字，调整按钮位置，隐去不用的按钮，绑定按钮事件
    /// </summary>
    /// <param name="model"></param>
    private void UpdateButton(Event model){
        // TODO
    }

    public void Close(int index) {
        _inAnimation = true;
        var coroutine = TweenUtility.Lerp(0.2f,
            () => _canvasGroup.alpha = 1,
            i => _canvasGroup.alpha = 1 - i,
            () => {
                _inAnimation = false;
                base.Close();
                UIManager.shared.OpenUI("UIResult", eventModel.ExplainArgsToAction(index));
                Destroy(gameObject);
            });
        StartCoroutine(coroutine());
    } 

    public void OnAnswerSelected(int index)
    {
        Close(index);
    }
}


