using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Event = Model.Event;
using TMPro;

public class EventDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] GameObject[] answerButtons;
    // Start is called before the first frame update
    void Start()
    {
        Event currentEvent = new Event(0);
        questionText.text = currentEvent.getQuestion();
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentEvent.getAnswer(i);
        }
        
    }

    public void OnAnswerSelected(int index)
    {
        //Event e = new Event(0);
        questionText.text = "pressed";
        //e.explainArgs(index);
    }
    
}


