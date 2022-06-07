using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartDialogue : MonoBehaviour
{
    public DialogueObject diaObj;
    void Start()
    {
        // DialogueController.Instance.talkPanel.SetActive(true);
        DialogueController.Instance.StartDialogue(diaObj);
    }
}
