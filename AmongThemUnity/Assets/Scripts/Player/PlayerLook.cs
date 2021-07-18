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
    private NavMeshAgentManager navMeshAgentManager;

    private float yRotation;
    private float xRotation;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance();
        navMeshAgentManager = NavMeshAgentManager.Instance();
        if (PlayerPrefs.HasKey("lookSensity"))
        {
            lookSensitivity = PlayerPrefs.GetFloat("lookSensity");
        }

        yRotation = 0f;
        xRotation = 0f;
    }


    public void RaycastInteractiveElement()
    {
        
        int layerMask = 1 << 8;

        
        layerMask = ~layerMask;
        
        if (!gm.PlayerCanClick)
        {
           return; 
        }
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f, layerMask))
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
                                if (gm.IsTutorial)
                                {
                                    gm.ReplacePlayerOnStartPosition();
                                }
                                else
                                {
                                    gm.EndGame(false);
                                    //Lose.SetActive(true);
                                }
                            }
                            else
                            {
                                NavMeshAgentManager.Instance().CopsGoOnCrimeScene();
                                gm.KillTarget();
                                if (gm.IsTutorial)
                                {
                                    TutorialManager.Instance().NextStep();
                                }
                            }
                        }
                    }
                    break;
                case "InteractiveElement":
                    Vector3 origin = hit.transform.position;
                    Destroy(hit.transform.gameObject);
                    break;
                case "TargetRoomDoor":
                    if (!gm.TargetIsAlive)
                    {
                        CodeMission.Instance().OpenMissionPanel();   
                    }
                    break;
                case "LaptopInfo":
                    PCMission.Instance().OpenMissionPanel();
                    break;
                case "ElevatorDoor":
                    if (!gm.DataRetrieve)
                    {
                        return;
                    }
                    if (gm.IsTutorial)
                    {
                        if (TutorialManager.Instance().GetStep() == 6)
                        {
                            gm.EndTutorial();
                            return;
                        }
                    }
                    gm.GoToNextFloor(hit.transform.parent.gameObject);
                    break;

            }
            
        }
    }
    
    
    public void SetRotationY(float yRot)
    {
        yRotation += yRot;
    }
    public void SetRotationX(float xRot)
    {
        xRotation = xRot;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance().PlayerCanRotate)
        {
            playerBody.RotateAround(playerBody.position, playerBody.up , yRotation * lookSensitivity /** Time.fixedDeltaTime*/);
            transform.localRotation = Quaternion.Euler(xRotation * lookSensitivity /** Time.fixedDeltaTime*/, 0f, 0f);
            
        }
        yRotation = 0f;
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
