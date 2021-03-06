﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This script is a general purpose explosion script, it plays audio, does physics kickback and detects obstacles.
 * Note that Rigidbodies are also detected as obstacles, if that is unwanted add detection for Rigidbodies yourself.
 * This is not standard functionality, because getComponent is relatively expensive and would have to be used very frequently.
 */

public class GenericExplosion : MonoBehaviour
{
    public float explosionRadius = 2f;
    public bool ignoreWalls = false;
    public string[] tagsToIgnore;
    public bool kickback = true;
    public float kickbackImpulse = 2f;
    public string[] tagsToIgnoreKickback;
    public float explosionDamage = 25f;
    
    public AudioClip explosionSound;
    public AudioSource audioSource;
    public float audioScale = 1f;

    public ParticleSystem particleSystem;

    public void TriggerExplosion()
    {
        
        if(explosionSound && audioSource)
            audioSource.PlayOneShot(explosionSound, audioScale);
        if(particleSystem)
            particleSystem.Play();
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider c in hitColliders)
        {
            bool ignore = false;
            foreach (string tag in tagsToIgnore)
            {
                if (c.tag == tag)
                    ignore = true;
            }
    
            if (!ignore)
            {
                //walls are ignored
                if (ignoreWalls)
                {
                    c.SendMessage("AddDamage", explosionDamage,SendMessageOptions.DontRequireReceiver);
                    if (kickback)
                    {
                        bool ignoreKick = false;
                        foreach (string tag in tagsToIgnoreKickback)
                        {
                            if (c.tag == tag)
                                ignoreKick = true;
                        }

                        if (!ignoreKick)
                        {
                            Rigidbody r = c.gameObject.GetComponent<Rigidbody>();
                            if (r)
                            {
                                Vector3 kickVector = c.transform.position - transform.position;
                                kickVector.Normalize();
                                r.AddForce(kickVector * kickbackImpulse, ForceMode.Impulse);
                            }
                        }
                    }
                }

                //walls are not ignored
                else
                {
                    Vector3 kickVector = c.transform.position - transform.position;
                    kickVector.Normalize();

                    RaycastHit hit;
                    Physics.Raycast(transform.position, kickVector, out hit, explosionRadius);
                    
                    //if only the target collider was hit with the detection ray
                    if (hit.collider == c)
                    {
                        c.SendMessage("AddDamage", explosionDamage,SendMessageOptions.DontRequireReceiver);
                        if (kickback)
                        {
                            bool ignoreKick = false;
                            foreach (string tag in tagsToIgnoreKickback)
                            {
                                if (c.tag == tag)
                                    ignoreKick = true;
                            }

                            if (!ignoreKick)
                            {
                                Rigidbody r = c.gameObject.GetComponent<Rigidbody>();
                                if (r)
                                {
                                    r.AddForce(kickVector * kickbackImpulse, ForceMode.Impulse);
                                }
                            }
                        }
                    }
                    
                }
                
                
            }
        }
        
        
    }
}