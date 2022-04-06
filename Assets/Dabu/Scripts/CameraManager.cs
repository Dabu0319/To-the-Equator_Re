using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    
    public CinemachineVirtualCamera PlayerCamera;
    
    public CinemachineVirtualCamera CutsceneCamera;
    public Animator cameraSwitch;

    public static CameraManager instance;

    public bool enterCutscene;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void SwitchState()
    // {
    //     if (enterCutscene)
    //     {
    //         StartCoroutine(StartSwitchState());
    //     }
    //
    //     enterCutscene = false;
    //
    // }
    public void SwitchState()
    {
        StartCoroutine(StartSwitchState());
    }

    public IEnumerator StartSwitchState()
    {
        PlayerManager.instance.PlayerDisabled();
        
        cameraSwitch.Play("Cutscene");
        yield return new WaitForSeconds(10f);
        cameraSwitch.Play("Cutscene02");
        yield return new WaitForSeconds(15f);
        cameraSwitch.Play("PlayerCamera");
        
        PlayerManager.instance.PlayerEnabled();
        
    }

    public void SwitchPriority()
    {
        StartCoroutine(StartSwitchPriority());
    }

    public IEnumerator StartSwitchPriority()
    {
        PlayerCamera.Priority = 0;
        CutsceneCamera.Priority = 1;
        yield return new WaitForSeconds(3f);
        PlayerCamera.Priority = 1;
        CutsceneCamera.Priority = 0;
        
        
    }
}
