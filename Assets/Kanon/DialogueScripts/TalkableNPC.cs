using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkableNPC : MonoBehaviour
{
    public DialogueObject dia;
    void OnTriggerEnter(Collider player)
    {
        if (!player.CompareTag("Player")) return;
        DialogueController.Instance.currentDialogue = dia;
        // DialogueController.Instance.ReadytoTalk();
    }
    void OnTriggerExit(Collider player)
    {
        if (!player.CompareTag("Player")) return;
        // DialogueController.Instance.LeaveReadytoTalk();
    }
}
