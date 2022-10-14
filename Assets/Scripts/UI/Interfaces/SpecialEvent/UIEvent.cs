using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Event = Model.Event;
using TMPro;
using UI;
using UI.Container;
using UI.Interfaces;
using UI.Interfaces.SpecialEvent;
using UnityEngine.UIElements;
using Utility;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class UIEvent : UIBase
{
    public UIContainerFlexBox container;
    public TextMeshProUGUI questionText;
    public Event eventModel;
    private CanvasGroup _canvasGroup;
    private bool _inAnimation;
    private UIButton[] _buttons;

    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _buttons = transform.GetComponentsInChildren<UIButton>();

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
    private void UpdateButton(Event model)
    {
        var curButtonInd = 0;
        foreach (var choice in model.choices)
        {
            if (choice.desc == "")
            {
                _buttons[curButtonInd].Show = false;
                curButtonInd++;
            }
            else
            {
                var answerButton = _buttons[curButtonInd];
                answerButton.Show = true;
                Debug.Log(curButtonInd);
                answerButton.Model = choice;
                curButtonInd++;
            }
        }
        container.UpdateLayout();

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
