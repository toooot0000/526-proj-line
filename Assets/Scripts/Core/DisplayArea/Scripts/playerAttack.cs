using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    private Animator myAnimator;

    private const string ATTACK_ANIM = "TriggerAttack";
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger(ATTACK_ANIM);
        }
    }
}
