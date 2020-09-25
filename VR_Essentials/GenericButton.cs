using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Generic button script, trigger the Click method to activate connected method (like door, laser, etc.)
 */

public class GenericButton : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Enables triggering if hit by GameObject with Collider and Rigidbody")]
    private bool triggerOnCollision = false;
    
    [Tooltip("The Functions to be triggered by this Button. Not all of them need to be used")]
    public UnityEvent onButtonClick, onButtonHold, onButtonRelease;


    public void Click()
    {
        onButtonClick.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            if (triggerOnCollision)
                onButtonClick.Invoke();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            if (triggerOnCollision)
                onButtonHold.Invoke();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            if (triggerOnCollision)
                onButtonRelease.Invoke();
        }
    }
}
