using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrigger : MonoBehaviour
{
    //0-disabled; 1-enabled;
    public int triggerType;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (triggerType)
            {
                case 0:
                    PlayerManager.instance.abilityActive = false;
                    break;
                case 1:
                    PlayerManager.instance.abilityActive = true;
                    break;
                
                    
            }
            Destroy(gameObject);
        }
    }
}
