using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName="Kanon/DialogueObject")]
[Serializable]
public class DialogueObject : ScriptableObject
{
    [Header("NPC图片")] public Sprite NPC_Image;
    [Header("NPC姓名")] public string NPC_Name;
    [Header("对话内容")] public List<string> dialogue;
    private int currentDialogueIndex = 0;

    public string GetCurrentDialogue()
    {
        if (currentDialogueIndex < dialogue.Count)
        {
            return dialogue[currentDialogueIndex++];
        }
        else
        {
            currentDialogueIndex = 0;
            return null;
        }
    }

    public void ResetCurrentDialogueIndex()
    {
        currentDialogueIndex = 0;
    }
}
