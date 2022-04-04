using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RespawnSystem
{
    public class RespawnHelper : MonoBehaviour
    {
        private RespawnManager m_RespawnManager;
        private void Awake()
        {
            m_RespawnManager = FindObjectOfType<RespawnManager>();
        }

        public void RespawnPlayer()
        {
            m_RespawnManager.Respawn(gameObject);
        }

        public void ResetPlayer()
        {
            m_RespawnManager.ResetAllSpawnPoint();
            m_RespawnManager.Respawn(gameObject);
        }
        
        
        
    }
}

