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
        Debug.Log(Camera.main.pixelWidth / 2);
    }

    // Update is called once per frame
    void Update()
    {
        GetMove();
        GetLook();
    }

    private void FixedUpdate()
    {
        if (touchIntentMove)
        {
            Vector3 offsetMove = touchMove - touchOriginMove;
            Vector3 direction = offsetMove.normalized;
            //Debug.Log($"{Input.mousePosition} - {touchMove} - {touchOriginMove} - {direction}");
            playerMove.SetDirection(direction);
        }
        else
        {
            playerMove.SetDirection(Vector3.zero);
        }

        if (touchIntentLook)
        {
            Vector3 offsetLook = touchLook - touchOriginLook;
            Vector3 look = offsetLook;//Vector3.ClampMagnitude(offsetLook, 1.0f);
            //Debug.Log($"{Input.mousePosition} - {touchLook} - {touchOriginLook} - {look}");
            playerLook.SetRotationY(Mathf.Clamp(look.x, -1f, 1f));
            playerLook.SetRotationX(Mathf.Clamp(-look.y, -90f, 90f));
        }
        else
        {
            playerLook.SetRotationY(0);
        }
    }

    void GetMove()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < Camera.main.pixelWidth / 2)
        {
            touchOriginMove = new Vector3(Input.mousePosition.x,0, Input.mousePosition.y);
        }

        if (Input.GetMouseButton(0) && touchOriginMove != Vector3.zero && touchOriginMove.x < Camera.main.pixelWidth / 2)
        {
            touchIntentMove = true;
            touchMove = new Vector3(Input.mousePosition.x,0, Input.mousePosition.y);
        }
        else
        {
            touchIntentMove = false;
            touchMove = Vector3.zero;
            touchOriginMove = Vector3.zero;
        }
    }
    
    void GetLook()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > Camera.main.pixelWidth / 2)
        {
            Debug.Log("oui");
            touchOriginLook = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }

        if (Input.GetMouseButton(0) && touchOriginLook != Vector3.zero && touchOriginLook.x > Camera.main.pixelWidth / 2)
        {
            touchIntentLook = true;
            touchLook = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
        else
        {
            touchIntentLook = false;
            touchLook = Vector3.zero;
            touchOriginLook = Vector3.zero;
        }
    }
}
