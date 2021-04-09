using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidController : MonoBehaviour
{
    [SerializeField] 
    private PlayerMove playerMove;
    [SerializeField] 
    private PlayerLook playerLook;
    
    
    private bool touchIntentMove;
    private Vector3 touchOriginMove;
    private Vector3 touchMove;
    private int indexTouchMove;
    private Touch inputMove;
    
    private bool touchIntentLook;
    private Vector3 touchOriginLook;
    private Vector3 touchLook;
    private int indexTouchLook;
    private Touch inputLook;

    private int nbTouch;
    private List<Touch> myTouch;

    public Text testText;
    public Text testText2;
    public Text testText3;
    
    
    private float mouseX;
    
    // Start is called before the first frame update
    void Start()
    {
        myTouch = new List<Touch>();
        touchIntentMove = false;
        nbTouch = 0;
        indexTouchLook = -1;
        indexTouchMove = -1;
    }

    // Update is called once per frame
    void Update()
    {
        testText.text = Input.touchCount.ToString();
        //testText2.text = $"{Input.touchCount}";
        GetMove();
        //GetLook();
        
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
        if (Input.touchCount > 0)
        {
            if (!myTouch.Contains(inputMove))
            {
                indexTouchMove = nbTouch;
            }
            inputMove = Input.GetTouch(indexTouchMove);
            testText2.text = inputMove.phase + " - " + touchOriginMove;
            testText3.text = inputMove.position.ToString();

            if (inputMove.phase == TouchPhase.Began && Input.mousePosition.x < Camera.main.pixelWidth / 2)
            {
                nbTouch++;
                myTouch.Add(inputMove);
                indexTouchMove = myTouch.IndexOf(inputMove);
                touchOriginMove = new Vector3(inputMove.position.x, 0, inputMove.position.y);
                //testText2.text = touchOriginMove.ToString();
            }
            
            if (touchOriginMove != Vector3.zero && (inputMove.phase == TouchPhase.Moved | inputMove.phase == TouchPhase.Stationary))
            {
                //inputMove = Input.GetTouch(nbTouch);
                touchIntentMove = true;
                touchMove = new Vector3(inputMove.position.x, 0, inputMove.position.y);
                testText3.text = inputMove.position.ToString();
            }
            if (inputMove.phase == TouchPhase.Ended)
            {
                myTouch.Remove(inputMove);
                nbTouch--;
                indexTouchMove = -1;
                touchIntentMove = false;
                touchMove = Vector3.zero;
                touchOriginMove = Vector3.zero;
                testText2.text = "None";
                testText3.text = "None";
            }
        }
        else
        {
            inputMove = new Touch();
        }
        
        /*if (Input.touchCount > 0 && Input.mousePosition.x < Camera.main.pixelWidth / 2 && touchOriginMove == Vector3.zero)
        {
            indexTouchMove = Input.touchCount - 1;
            inputMove = Input.GetTouch(indexTouchMove);
            testText2.text = inputMove.position.ToString();
            touchOriginMove = new Vector3(inputMove.position.x,0, inputMove.position.y);
        }

        if (Input.touchCount > 0 && indexTouchMove >= 0 && touchOriginMove != Vector3.zero && inputMove.position.x < Camera.main.pixelWidth / 2)
        {
            touchIntentMove = true;
            inputMove = Input.GetTouch(indexTouchMove);
            testText3.text = inputMove.position.ToString();
            touchMove = new Vector3(inputMove.position.x,0, inputMove.position.y);
        }
        else
        {
            indexTouchMove = -1;
            indexTouchLook--;
            touchIntentMove = false;
            touchMove = Vector3.zero;
            touchOriginMove = Vector3.zero;
        }*/
    }
    
    void GetLook()
    {
        if (Input.touchCount > 0)
        {
            inputLook = Input.GetTouch(nbTouch);

            if (inputLook.phase == TouchPhase.Began && Input.mousePosition.x > Camera.main.pixelWidth / 2)
            {
                nbTouch++;
                touchOriginLook = new Vector3(inputLook.position.x, inputLook.position.y, 0);
            }

            if (inputLook.phase == TouchPhase.Ended)
            {
                nbTouch--;
                touchIntentLook = false;
                touchLook = Vector3.zero;
                touchOriginLook = Vector3.zero;
            }

            else
            {
                touchIntentLook = true;
                touchLook = new Vector3(inputLook.position.x, inputLook.position.y, 0);
            }
        }
        /*if (Input.touchCount > 0 && Input.mousePosition.x > Camera.main.pixelWidth / 2 && touchOriginLook == Vector3.zero)
        {
            nbTouch--;
            indexTouchLook = Input.touchCount - 1;
            inputLook = Input.GetTouch(indexTouchLook);
            touchOriginLook = new Vector3(inputLook.position.x, inputLook.position.y, 0);
        }

        if (Input.touchCount > 0 && indexTouchLook >= 0 && touchOriginLook != Vector3.zero && inputLook.position.x > Camera.main.pixelWidth / 2)
        {
            inputLook = Input.GetTouch(indexTouchLook);
            touchIntentLook = true;
            touchLook = new Vector3(inputLook.position.x, inputLook.position.y, 0);
        }
        else
        {
            indexTouchLook = -1;
            indexTouchMove--;
            touchIntentLook = false;
            touchLook = Vector3.zero;
            touchOriginLook = Vector3.zero;
        }*/
    }
}
