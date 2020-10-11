using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton implementation for game managers

public class Singleton : MonoBehaviour
{
    public static Singleton instance;
    
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }

}
