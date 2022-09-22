using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSetTrigger : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("playAttack", true);
        }
        else
        {
            anim.SetBool("playAttack", false);
        }
    }
}
