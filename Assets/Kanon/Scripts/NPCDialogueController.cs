using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogueController : UnitySingleton<NPCDialogueController>
{
    public Transform NPCDiaTrans;
    public Text npcDiaText;

    public Transform npcTrans;

    private bool isTalking;
    
    public DialogueObject currentDialogue;

    public Vector3 offset = Vector3.zero;

    private void Update()
    {
        if (isTalking)
        {
            NPCDiaTrans.position = Camera.main.WorldToScreenPoint(npcTrans.position) + offset;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayDialogue();
                return;
            }
        }
    }

    public void NPCDialogueStart(DialogueObject dia)
    {
        isTalking = true;
        NPCDiaTrans.gameObject.SetActive(true);
        
        currentDialogue = dia;
        dia.ResetCurrentDialogueIndex();
        PlayDialogue();
    }
    
    public void NPCDialogueEND()
    {
        isTalking = false;
        NPCDiaTrans.gameObject.SetActive(false);
    }
    
    public void PlayDialogue()
    {
        string curDia = currentDialogue.GetCurrentDialogue();
        if (curDia != null)
        {
            npcDiaText.text = curDia;
        }
        else
        {
            NPCDialogueEND();
        }
    }
}
