using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/*
 * Script for toggle-buttons and levers
 * Not physics interaction compatible yet
 */

public class ToggleLever : MonoBehaviour
{
    
    public UnityEvent turnedOn, turnedOff;
    public bool on;

    public void Activate()
    {
        on = !on;

        if (on)
        {
            turnedOn.Invoke();
        }else
        {
            turnedOff.Invoke();
        }
    }
    
}
