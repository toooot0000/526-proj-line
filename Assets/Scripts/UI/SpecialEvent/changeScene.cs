using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class changeScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject changeButton;
    void Start()
    {
        // TextMeshProUGUI buttonText = changeButton.GetComponentInChildren<TextMeshProUGUI>();
        // buttonText.text = "go to scene 2";
    }

    public void OnSelectedButton(int i)
    {
        if (i == 1)
        {
            SceneManager.LoadScene("SpecialEvent");
        }

        if (i == 2)
        {
            SceneManager.LoadScene("Main");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
