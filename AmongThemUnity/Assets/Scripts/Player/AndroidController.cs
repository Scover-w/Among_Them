using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidController : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystickMove;
    public VariableJoystick variableJoystickLook;
    public Transform player;    
    
    [SerializeField] 
    private PlayerMove playerMove;
    [SerializeField] 
    private PlayerLook playerLook;

    [SerializeField] private Button actionButton;

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
        actionButton.gameObject.SetActive(true);
        myTouch = new List<Touch>();
        nbTouch = 0;
        indexTouchLook = -1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        actionButton.onClick.AddListener(delegate { playerLook.RaycastInteractiveElement(); });
    }

    private void Update()
    {
        GetLook();
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

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > Camera.main.pixelWidth / 2)
            {
                
                touchOriginLook = new Vector3(inputLook.position.x, inputLook.position.y, 0);
                //Debug.Log(touchOriginLook);
            }
        }
    }
    

    public void FixedUpdate()
    {
        
        
        Vector3 direction = Vector3.forward * variableJoystickMove.Vertical + Vector3.right * variableJoystickMove.Horizontal;
        playerMove.SetDirection(direction);

        float rotationX = -variableJoystickLook.Vertical; 
        float rotationY = variableJoystickLook.Horizontal;
        playerLook.SetRotationX(rotationX);
        playerLook.SetRotationY(rotationY);

        //player.transform.position += direction * speed * Time.fixedDeltaTime;
        
        //rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        
    }
}
