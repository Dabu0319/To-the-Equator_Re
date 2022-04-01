using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float m_TimeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject echo;

    private PlayerMovement m_PlayerMovement;
    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            if (m_TimeBtwSpawns <= 0)
            {
                GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(instance,8f);
                m_TimeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                m_TimeBtwSpawns -= Time.deltaTime;
            }
        }

    }
    
    
}
