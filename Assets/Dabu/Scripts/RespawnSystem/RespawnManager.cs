using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RespawnSystem
{
    public class RespawnManager : MonoBehaviour
    {
        private List<RespawnPoint> m_RespawnPoints = new List<RespawnPoint>();

        private RespawnPoint currentRespawnPoint;

        private void Awake()
        {
            foreach (Transform item in transform)
            {
                m_RespawnPoints.Add(item.GetComponent<RespawnPoint>());
            }

            currentRespawnPoint = m_RespawnPoints[0];
        }

        public void UpdateRespawnPoint(RespawnPoint newRespawnPoint)
        {
            currentRespawnPoint.DisableRespawnPoint();
            currentRespawnPoint = newRespawnPoint;
        }

        public void Respawn(GameObject objectToRespawn)
        {
            currentRespawnPoint.RespawnPlayer();
            objectToRespawn.SetActive(true);
        }

        public void RespawnAt(RespawnPoint spawnPoint, GameObject playerGo)
        {
            spawnPoint.SetPlayerGo(playerGo);
            Respawn(playerGo);
        }

        public void ResetAllSpawnPoint()
        {
            foreach (var item in m_RespawnPoints)
            {
                item.ResetRespawnPoint();
            }

            currentRespawnPoint = m_RespawnPoints[0];
        }
        


    }
}

