using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    [SerializeField] 
    private PlayerMove playerMove;
    [SerializeField] 
    private PlayerLook playerLook;
    
    
    private bool touchIntentMove;
    private Vector3 touchOriginMove;
    private Vector3 touchMove;
    
    private bool touchIntentLook;
    private Vector3 touchOriginLook;
    private Vector3 touchLook;
    
    
    private float mouseX;
    
    // Start is called before the first frame update
    void Start()
    {
        touchIntentMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetMove();
        GetLook();
        
        mouseX = Input.GetAxis("Mouse X");
        playerLook.SetRotationX(mouseX);
    }

    private void FixedUpdate()
    {
        if (touchIntentMove)
        {
            Vector3 offsetMove = touchMove - touchOriginMove;
            Vector3 direction = Vector3.ClampMagnitude(offsetMove, 1.0f);
            playerMove.SetDirection(direction);
        }
        else
        {
            playerMove.SetDirection(Vector3.zero);
        }

        if (touchIntentLook)
        {
            Vector3 offsetLook = touchLook - touchOriginLook;
            Vector3 look = Vector3.ClampMagnitude(offsetLook, 1.0f);
            playerLook.SetRotationX(look.x);
            playerLook.SetRotationY(look.y);
        }
    }

    void GetMove()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < Camera.main.pixelWidth / 2)
        {
            touchOriginMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,0, Input.mousePosition.y));
        }

        if (Input.GetMouseButton(0))
        {
            touchIntentMove = true;
            touchMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,0, Input.mousePosition.y
            ));
        }
        else
        {
            touchIntentMove = false;
        }
    }
    
    void GetLook()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > Camera.main.pixelWidth / 2)
        {
            touchOriginLook = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }

        if (Input.GetMouseButton(0))
        {
            touchIntentLook = true;
            touchLook = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }
        else
        {
            touchIntentLook = false;
        }
    }
}
