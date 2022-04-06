using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestroyFallingObjects : MonoBehaviour
{


    public LayerMask objectsToDestroyLayerMask;
    public Vector2 size;
    
    [Header("Gizmo parameters")]
    public Color gizmoColor = Color.red;
    public bool showGizmo = true;

    private void FixedUpdate()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, size, 0, objectsToDestroyLayerMask);

        if (collider != null)
        {
            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            if (player == null)
            {
                Destroy(collider.gameObject);
                return;
            }
            //
            player.PlayerDied();
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position,size);
        }
    }
}
