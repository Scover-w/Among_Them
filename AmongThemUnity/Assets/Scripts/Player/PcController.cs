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
    
    // Running
    private float multiplyRunningValue = 1.60f;
    private float runningDuration = 3f;
    private float halfRunningDuration;
    private bool isRunning;
    private float runningTimer;

    private float screenRatio;
    private void Start()
    {
#if !UNITY_STANDALONE
        Destroy(this);
#endif
        Cursor.lockState = CursorLockMode.Locked;
        runningTimer = runningDuration;
        halfRunningDuration = runningDuration / 2f;
        screenRatio = ((Screen.width / Screen.height - 1) / 2) + 1;
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
        mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime;
        mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime;
        
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

        if (GameInputManager.GetKey("Shift"))
        {
            if (!isRunning && runningTimer < halfRunningDuration)
            {
                isRunning = false;
                runningTimer = Mathf.Clamp(runningTimer + Time.fixedDeltaTime * 0.5f, 0f, runningDuration);
            }
            else if (runningTimer <= 0f)
                isRunning = false;
            else
            {
                isRunning = true;
                runningTimer = Mathf.Clamp(runningTimer - Time.fixedDeltaTime, 0f, runningDuration);
            }
               
        }
        else
        {
            isRunning = false;
            runningTimer = Mathf.Clamp(runningTimer + Time.fixedDeltaTime * 0.5f, 0f, runningDuration);
        }

        if (Input.mouseScrollDelta.y != 0f)
        {
            InventoryUI.instance.SelectOrb((int)Math.Abs(Input.mouseScrollDelta.y));
        }
        
        directionMove = directionMove.normalized;
        playerMove.SetDirection(directionMove * (isRunning? multiplyRunningValue : 1f));
    }
    public void ClickToAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerLook.RaycastInteractiveElement();
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance().UseOrb(InventoryUI.instance.GetSelectedOrb());
        }
    }
}
