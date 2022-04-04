using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RespawnSystem
{
    
    public class RespawnPoint : MonoBehaviour
    {

        public GameObject respawnTarget;
        
        [field:SerializeField]
        private UnityEvent onSpawnPointActivated { get; set; }

        private void Start()
        {
            onSpawnPointActivated.AddListener(()=>
                GetComponentInParent<RespawnManager>().UpdateRespawnPoint(this)
                );
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.respawnTarget = other.gameObject;
                onSpawnPointActivated?.Invoke();
                //GetComponent<Collider2D>().enabled = false;

            }
        }

        // public void RespawnPlayer()
        // {
        //     respawnTarget.transform.position = transform.position;
        // }
        public void RespawnPlayer() => respawnTarget.transform.position = transform.position;


        public void SetPlayerGo(GameObject player)
        {
            respawnTarget = player;
            GetComponent<Collider2D>().enabled = false;
        }

        public void DisableRespawnPoint()
        {
            GetComponent<Collider2D>().enabled = false;
        }

        public void ResetRespawnPoint()
        {
            respawnTarget = null;
            GetComponent<Collider2D>().enabled = true;
        }
        
    }
    
    

}
