﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContinueButton : MonoBehaviour
{
    GoogleAd googleAD;
    bool isPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        googleAD = FindObjectOfType<GoogleAd>();

        isPressed = false;
    }

    public void TouchUp()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.audioClick, 1f);
        isPressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            
        }
    }
}