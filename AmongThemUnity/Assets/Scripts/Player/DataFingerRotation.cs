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
    private float lastDragX;
    private float lastDragY;
    private int idTouch;
    private bool isTouched = false;

    private void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        Debug.Log("Start DataFinger");
    }


    void Update()
    {
        if (!isTouched)
        {
            if (Input.touchCount > 0)
            {
                int i = Input.touchCount;
                if(i != 0)
                    Debug.Log("Touch nb : " + i);
                
                for (int j = 0; j < i; j++)
                {
                    if (Input.GetTouch(j).phase == TouchPhase.Began && Input.GetTouch(j).position.x > (screenWidth / 2.3))// Click on the right side of the screen
                    {
                        dragOrigin = Input.GetTouch(j).position;
                        idTouch = Input.GetTouch(j).fingerId;
                        isTouched = true;
                        dragX = 0;
                        dragY = 0;
                        lastDragX = 0;
                        lastDragY = 0;
                        Debug.Log("Right click");
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

                dragOrigin = Input.GetTouch(k).position;
                
                dragX = dragFromOrigin.x / 10f;
                dragY = dragFromOrigin.y / 10f;

                xRotation = (dragX - lastDragX);
                yRotation = (dragY - lastDragY);
                
                lastDragX = dragX;
                lastDragY = dragX;

                Debug.Log("Rotation : x -> " + xRotation + " , y-> " + yRotation);
                playerLook.SetRotation(xRotation, -yRotation);
            }
            else if(Input.GetTouch(k).phase == TouchPhase.Ended)
            {
                isTouched = false;
            }
        }
    }
}
