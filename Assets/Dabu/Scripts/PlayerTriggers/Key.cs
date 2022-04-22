using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    //KeyType:
    //0-SetActive(false);
    //1-SetActive(true);
    //2-Key
    public int keyType;
    public GameObject interactObj;

    // public Sprite activeSprite;
    //
    // public Sprite originSprite;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //originSprite = GetComponent<SpriteRenderer>().sprite;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.CompareTag("PlayerShadow") || other.CompareTag("Player") || other.CompareTag("Box") ||other.CompareTag("BoxShadow"))
        {
            //GetComponent<SpriteRenderer>().sprite = activeSprite;
            
            switch (keyType)
            {
                case 0:
                    interactObj.SetActive(false);
                    Destroy(gameObject);
                    break;
                case 1:
                    interactObj.SetActive(true);
                    Destroy(gameObject);
                    break;
                case 2:
                    interactObj.GetComponent<Animator>().SetBool("DoorOpen",true);
                    interactObj.GetComponent<Animator>().SetBool("DoorClose",false);
                    anim.SetBool("KeyEnable",true);
                    anim.SetBool("KeyDisable",false);
                    break;
                  
                    
            }
            

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //GetComponent<SpriteRenderer>().sprite = originSprite;
        
        if (other.CompareTag("PlayerShadow") || other.CompareTag("Player") || other.CompareTag("Box") ||
            other.CompareTag("BoxShadow"))
        {
            //GetComponent<SpriteRenderer>().sprite = activeSprite;
            
            switch (keyType)
            {
                case 2:
                    interactObj.GetComponent<Animator>().SetBool("DoorOpen",false);
                    interactObj.GetComponent<Animator>().SetBool("DoorClose",true);
                    anim.SetBool("KeyEnable",false);
                    anim.SetBool("KeyDisable",true);
                    Debug.Log("Exit");
                    break;
                
            }
        }

    }
}
