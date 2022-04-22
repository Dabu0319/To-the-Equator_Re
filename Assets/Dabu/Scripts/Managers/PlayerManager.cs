using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum PlayerStatus
{
    PlayerActive,
    PlayerShadowActive,
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    public GameObject player;
    public GameObject playerShadow;

    public CinemachineVirtualCamera cineCam;

    public PlayerStatus playerStatus = PlayerStatus.PlayerActive;

    public bool abilityActive;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //cineCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (abilityActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)  && playerStatus==PlayerStatus.PlayerActive)
            {
            
            
                Instantiate(playerShadow,player.transform.position,player.transform.rotation);
                player.GetComponent<PlayerMovement>().moveEnabled = false;
                playerShadow.GetComponent<PlayerMovement>().moveEnabled = true;

                cineCam.Follow = GameObject.FindWithTag("PlayerShadow").transform;
                cineCam.LookAt = GameObject.FindWithTag("PlayerShadow").transform;
            
                playerStatus = PlayerStatus.PlayerShadowActive;
            }
        
            else if (Input.GetKeyDown(KeyCode.LeftShift)  && playerStatus==PlayerStatus.PlayerShadowActive)
            {
            
            
                //DestroyImmediate (playerShadow, true);
                Destroy(GameObject.FindWithTag("PlayerShadow"));
                player.GetComponent<PlayerMovement>().moveEnabled = true;
                
            
                cineCam.Follow = player.transform;
                cineCam.LookAt = player.transform;
            
                playerStatus = PlayerStatus.PlayerActive;
            }
        }

        
    }

    public void PlayerDisabled()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        abilityActive = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

    }

    public void PlayerEnabled()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        abilityActive = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        
    }
}
