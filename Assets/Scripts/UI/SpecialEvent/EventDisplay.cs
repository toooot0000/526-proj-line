using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Event = Model.Event;
using TMPro;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class EventDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField]Sprite defaultAnswerSprite;
    [SerializeField]Sprite selectedAnswerSprite;
    [SerializeField] int eventIndex = 4;

    private void Awake()
    {
        //eventIndex = Random.Range(0, 3);
        //eventIndex = 4;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Event currentEvent = new Event(eventIndex);
        questionText.text = currentEvent.getQuestion();
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentEvent.getAnswer(i);
        }
        
        Debug.Log(GameManager.shared.game.player.gears.Count);

    }

    public void OnAnswerSelected(int index)
    {
        Debug.Log("pressed");
        Event e = new Event(eventIndex);
        questionText.text = index.ToString();
        e.explainArgs(index);
        Image buttonImage = answerButtons[index].GetComponent<Image>();
        buttonImage.sprite = selectedAnswerSprite;
        Debug.Log(GameManager.shared.game.player.gears.Count);
        
    }
    
    
}


