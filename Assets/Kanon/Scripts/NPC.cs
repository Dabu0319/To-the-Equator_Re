using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueObject dia;
    private void OnTriggerEnter2D(Collider2D player)
    {
        if(!player.CompareTag("Player")) return;
        
        Debug.Log("PlayerEnter");

        NPCDialogueController.Instance.npcTrans = this.transform;
        
        NPCDialogueController.Instance.NPCDialogueStart(dia);
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if(!player.CompareTag("Player")) return;
        
        Debug.Log("PlayerExit");
        
        NPCDialogueController.Instance.NPCDialogueEND();
    }
}
