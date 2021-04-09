using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcController : MonoBehaviour
{
    [SerializeField] 
    private PlayerMove playerMove;

    [SerializeField] 
    private PlayerLook playerLook;

    // Rotation
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    
    // Movement
    private float xMove;
    private float zMove;
    private Vector3 directionMove;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        SendRotationValues();
        SendMovementValues();

        ClickToAction();
    }

    public void SendRotationValues()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        playerLook.SetRotationY(mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerLook.SetRotationX(xRotation);
    }
    public void SendMovementValues()
    {
        directionMove = Vector3.zero;

        if (GameInputManager.GetKey("Right"))
        {
            directionMove += Vector3.right;
        }

        if (GameInputManager.GetKey("Left"))
        {
            directionMove -= Vector3.right;
        }

        if (GameInputManager.GetKey("Forward"))
        {
            directionMove += Vector3.forward;
        }

        if (GameInputManager.GetKey("Backward"))
        {
            directionMove -= Vector3.forward;
        }

        directionMove = directionMove.normalized;
        playerMove.SetDirection(directionMove);
    }
    public void ClickToAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerLook.RaycastInteractiveElement();
        }
    }
}
