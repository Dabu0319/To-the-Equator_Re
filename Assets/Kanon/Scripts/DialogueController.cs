using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : UnitySingleton<DialogueController>
{
    public DialogueObject currentDialogue;
    
    //不论对话内容的action
    public UnityAction OnDialogueStart;
    public UnityAction onDialogueEnd;
    
    [Header("手动配置")]
    public Image npc_image;
    public Text npc_name;
    public Text npc_dialogue;
    public GameObject talkPanel;
    private bool _isTalking = false;

    private void Awake()
    {
        //talkPanel.SetActive(false);
        
        OnDialogueStart = () =>
        {
            Debug.Log("startTalk");
            _isTalking = true;
            talkPanel.SetActive(true);
        };
        onDialogueEnd = () =>
        {
            Debug.Log("EndTalk");
            _isTalking = false;
            talkPanel.SetActive(false);
        };
        
        talkPanel.SetActive(false);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (_isTalking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayDialogue();
                return;
            }
        }
    }

    public void StartDialogue(DialogueObject dia)
    {
        if(OnDialogueStart != null) OnDialogueStart.Invoke();
        currentDialogue = dia;
        currentDialogue.ResetCurrentDialogueIndex();
        npc_image.sprite = currentDialogue.NPC_Image;
        npc_name.text = currentDialogue.NPC_Name;
        PlayDialogue();
    }

    public void PlayDialogue()
    {
        //每调用一次播放一句话
        // if(currentDialogue == null) return; 
        string curDia = currentDialogue.GetCurrentDialogue();
        if (curDia != null)
        {
            npc_dialogue.text = curDia;
        }
        else
        {
            //没有话说的时候，调用退出对话。
            if(onDialogueEnd != null) onDialogueEnd.Invoke();
        }
    }
}
