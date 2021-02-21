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
            switch (hit.transform.gameObject.tag)
            {
                case "ToKillAgent":
                    if (hit.transform.gameObject.name.Contains("ToKillAgent"))
                    {
                        if (hit.distance < 2.0f)
                        {
                            if (IsSomeoneWatching())
                            {
                                Debug.Log("Lose");
                            }
                            else
                            {
                                Debug.Log("Win");
                            }
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
}
