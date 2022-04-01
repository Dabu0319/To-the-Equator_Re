using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private TrailRenderer trail;
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //trail.startColor = Color.green;
            trail.startColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            trail.endWidth = 1;
            trail.endColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos;
    }
}
