using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Brackeys : MonoBehaviour
{
    [Header("PlayerID")] 

    
    private CharacterController2D _controller2D;

    public float _horizontalMove;
    public bool _jump;

    public float runSpeed;
    void Start()
    {
        _controller2D = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        _controller2D.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
        _jump = false;
    }
}
