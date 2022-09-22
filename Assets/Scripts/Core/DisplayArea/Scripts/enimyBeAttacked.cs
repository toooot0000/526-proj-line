using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enimyBeAttacked : MonoBehaviour
{
    private Animator myAnimator;

    private const string BE_ATTACKED_ANIM = "TriggerBeAttacked";
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger(BE_ATTACKED_ANIM);
        }
    }
}
