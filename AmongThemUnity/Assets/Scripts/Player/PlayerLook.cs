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

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance();
        navMeshAgentManager = NavMeshAgentManager.Instance();
        if (PlayerPrefs.HasKey("lookSensity"))
        {
            lookSensitivity = PlayerPrefs.GetFloat("lookSensity");
        }
    }


    public void RaycastInteractiveElement()
    {
        if (!gm.PlayerCanClick)
        {
           return; 
        }
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
                                if (gm.IsTutorial)
                                {
                                    gm.ReplacePlayerOnStartPosition();
                                }
                                else
                                {
                                    Lose.SetActive(true);
                                }
                            }
                            else
                            {
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
                    gm.GetNextTargetInformation();
                    if (gm.IsTutorial)
                    {
                        TutorialManager.Instance().TPinHall();
                        TutorialManager.Instance().NextStep();
                    }
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
                    gm.GoToNextFloor();
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
        if (GameManager.Instance().PlayerCanRotate)
        {
            playerBody.RotateAround(playerBody.position, playerBody.up , yRotation * lookSensitivity * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(xRotation * lookSensitivity * Time.fixedDeltaTime, 0f, 0f);
            
        }
            
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
