using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrigger : MonoBehaviour
{
    public float windTime = 3;
    public float windInterval = 6f;
    public float startDelay = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("PlayerShadow"))
        {
            GameManager.instance.StartCoroutine(GameManager.instance.EnterWind(startDelay,windTime, windInterval));
            
        }
        
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")|| col.CompareTag("PlayerShadow"))
        {
            GameManager.instance.StopWind();
            
        }
        
    }
}
