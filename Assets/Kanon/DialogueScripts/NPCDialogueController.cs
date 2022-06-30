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



    //public float wordSpeed;//让字母一个个打出来



    private void Update()
    {
        if (isTalking)
        {
            NPCDiaTrans.position = Camera.main.WorldToScreenPoint(npcTrans.position) + offset;
            if (Input.GetKeyDown(KeyCode.I))
            {
                PlayDialogue();
                return;
            }
        }
    }



    //IEnumerator Typing()//让字母一个个打出来
    //{
        //foreach (char letter in currentDialogue.GetCurrentDialogue())
        //{
           // npcDiaText.text += letter;
           // yield return new WaitForSeconds(wordSpeed);
       // }
    //}



    public void NPCDialogueStart(DialogueObject dia)
    {
        if(isTalking) return;
        isTalking = true;
        
        NPCDiaTrans.gameObject.SetActive(true);
        
        currentDialogue = dia;
        dia.ResetCurrentDialogueIndex();
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
            //StartCoroutine(Typing());//让字母一个个打出来
        }
        else
        {
            NPCDialogueEND();
        }
    }
}
