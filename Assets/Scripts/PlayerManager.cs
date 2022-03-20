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
    public GameObject player;
    public GameObject playerShadow;

    public CinemachineVirtualCamera cineCam;

    public PlayerStatus playerStatus = PlayerStatus.PlayerActive;
    void Start()
    {
        cineCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)  && playerStatus==PlayerStatus.PlayerActive)
        {
            
            
            Instantiate(playerShadow,player.transform.position,player.transform.rotation);
            player.GetComponent<PlayerMovement>().enabled = false;

            cineCam.Follow = GameObject.FindWithTag("PlayerShadow").transform;
            cineCam.LookAt = GameObject.FindWithTag("PlayerShadow").transform;
            
            playerStatus = PlayerStatus.PlayerShadowActive;
        }
        
        else if (Input.GetKeyDown(KeyCode.LeftShift)  && playerStatus==PlayerStatus.PlayerShadowActive)
        {
            
            
            //DestroyImmediate (playerShadow, true);
            Destroy(GameObject.FindWithTag("PlayerShadow"));
            player.GetComponent<PlayerMovement>().enabled = true;
            
            cineCam.Follow = player.transform;
            cineCam.LookAt = player.transform;
            
            playerStatus = PlayerStatus.PlayerActive;
        }
        
    }
}
