using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedResolution : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(390, 844, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
