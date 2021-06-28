using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] 
    private GameObject reference;

    [SerializeField] 
    private Rigidbody playerR;
    
    [SerializeField] 
    private float speed = 2.0f;

    private Vector3 velocity;
    private bool isGrounded;
    private float gravity = -9.81f;
    
    private Vector3 _direction;
    private Vector3 _relativeDirection;
    
    private bool canMove = true;
    
    public void SetDirection(Vector3 dir)
    {
        _direction = dir;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            playerR.angularVelocity = Vector3.zero;
            MoveCharacter(_direction);
            //ApplyGravity();
        }
    }

  
    private void MoveCharacter(Vector3 absoluteDirection)
    {
        _relativeDirection = transform.right * absoluteDirection.x + transform.forward * absoluteDirection.z;
        playerR.MovePosition(playerR.position  + _relativeDirection * (Time.fixedDeltaTime * speed));
    }
    
    public void EnableMove()
    {
        canMove = true;
    }
    
    public void DisableMove()
    {
        canMove = false;
    }
}
