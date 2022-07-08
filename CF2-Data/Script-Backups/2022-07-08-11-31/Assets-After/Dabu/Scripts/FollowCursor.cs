using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private TrailRenderer trail;
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        ControlFreak2.CFCursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
        {
            //trail.startColor = Color.green;
            trail.startColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            trail.endWidth = 1;
            trail.endColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        Vector2 pos = Camera.main.ScreenToWorldPoint(ControlFreak2.CF2Input.mousePosition);
        transform.position = pos;
    }
}
