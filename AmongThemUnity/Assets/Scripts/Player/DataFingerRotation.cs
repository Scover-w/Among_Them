using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFingerRotation : MonoBehaviour
{
    [SerializeField] 
    private PlayerLook playerLook;
    
    private float screenWidth;
    private float screenHeight;

    private Vector2 dragOrigin;
    private Vector2 dragFromOrigin;

    private float xRotation;
    private float yRotation;
    private float dragX;
    private float dragY;
    private float lastDragY;
    private float lastDragX;
    private int idTouch;
    private bool isTouched = false;

    private void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
    
    void Update()
    {
        if (!isTouched)
        {
            if (Input.touchCount > 0)
            {
                int i = Input.touchCount;
                
                for (int j = 0; j < i; j++)
                {
                    if (Input.GetTouch(j).phase == TouchPhase.Began && Input.GetTouch(j).position.x > (screenWidth / 2.3))// Click on the right side of the screen
                    {
                        dragOrigin = Input.GetTouch(j).position;
                        idTouch = Input.GetTouch(j).fingerId;
                        isTouched = true;
                        dragX = 0f;
                        dragY = 0f;
                        lastDragX = 0f;
                        lastDragY = 0f;
                        return;
                    }
                }
            }
        }
        else
        {
            int k = 0;
            while (Input.GetTouch(k).fingerId != idTouch)
            {
                k++;
            }
            
            if(Input.GetTouch(k).phase == TouchPhase.Moved)
            {
                dragFromOrigin = Input.GetTouch(k).position - dragOrigin;
                dragX = dragFromOrigin.x * 100f / screenWidth;
                dragY = dragFromOrigin.y * 100f / screenHeight;

                xRotation = lastDragX - dragX;
                yRotation = lastDragY - dragY;

                lastDragX = dragX;
                lastDragY = dragY;
                
                playerLook.SetRotationX(xRotation * 2);
                playerLook.SetRotationY(yRotation * 2);
            }
            else if(Input.GetTouch(k).phase == TouchPhase.Ended)
            {
                isTouched = false;
            }
        }
    }
}
