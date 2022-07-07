using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public bool playerIn;
    public DialogueObject dia;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && playerIn)
        {
            NPCDialogueController.Instance.NPCDialogueStart(dia);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerShadow"))
        {
            playerIn = true;
            NPCDialogueController.Instance.npcTrans = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerShadow"))
        {
            playerIn = false;
            NPCDialogueController.Instance.NPCDialogueEND();
        }
    }
}
