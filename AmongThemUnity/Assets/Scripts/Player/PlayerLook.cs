using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] 
    private float lookSensitivity = 100f;

    [SerializeField] 
    private Transform playerBody;
    
    [SerializeField]
    Camera cam;

    [SerializeField] 
    private PlayerMove playerMove;

    [SerializeField] 
    private GameObject Win;
    
    [SerializeField] 
    private GameObject Lose;

    [SerializeField] 
    private NavMeshAgentManager navMeshAgentManager;

    private float yRotation;
    private float xRotation;

    private bool canRotate = true;


    public void RaycastInteractiveElement()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f))
        {
            Debug.Log(hit.transform.gameObject.name);
            switch (hit.transform.gameObject.tag)
            {
                case "ToKillAgent":
                    if (hit.transform.gameObject.tag.Contains("ToKillAgent"))
                    {
                        if (hit.distance < 2.0f)
                        {
                            
                            if (IsSomeoneWatching())
                            {
                                Win.SetActive(false);
                                Lose.SetActive(true);
                            }
                            else
                            {
                                Win.SetActive(true);
                                Lose.SetActive(false);
                            }
                            Time.timeScale = 0f;
                            Cursor.lockState = CursorLockMode.None;
                        }
                    }
                    else
                    {
                        // Nothing happen yet
                    }
                    break;
                case "InteractiveElement":
                    Vector3 origin = hit.transform.position;
                    Destroy(hit.transform.gameObject);
                    break;
 
            }
            
        }
    }
    
    
    public void SetRotationY(float yRot)
    {
        yRotation = yRot;
    }
    public void SetRotationX(float xRot)
    {
        xRotation = xRot;
    }

    private void FixedUpdate()
    {
        if (canRotate)
        {
            playerBody.RotateAround(playerBody.position, playerBody.up , yRotation * lookSensitivity * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(xRotation * lookSensitivity * Time.fixedDeltaTime, 0f, 0f);
            
        }
            
    }

    public void EnableRotation()
    {
        canRotate = true;
    }
    
    public void DisableRotation()
    {
        canRotate = false;
    }

    public bool IsSomeoneWatching()
    {
        return navMeshAgentManager.IsSomeoneWatching();
    }

    public float GetSensitivity()
    {
        return lookSensitivity;
    }

    public void SetSensitivity(float sensitivity)
    {
        lookSensitivity = sensitivity;
    }
}
