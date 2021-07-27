using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerLook : MonoBehaviour
{
    public static PlayerLook instance;
    
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

    private GameManager gameManager;

    private Vector2 middleScreen;

    private bool canRotate = false;
    private bool canClick = false;

    private void Start()
    {
        instance = this;
        gameManager = GameManager.Instance();
        navMeshAgentManager = NavMeshAgentManager.Instance();
        if (PlayerPrefs.HasKey("lookSensity"))
        {
            lookSensitivity = PlayerPrefs.GetFloat("lookSensity");
        }

#if !UNITY_STANDALONE
        middleScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
#endif

        yRotation = 0f;
        xRotation = 0f;
    }


    public void RaycastInteractiveElement()
    {
        
        int layerMask = 1 << 8;

        
        layerMask = ~layerMask;
        
        if (!canClick)
        {
           return; 
        }
        
        Ray ray;
#if UNITY_STANDALONE
        ray = cam.ScreenPointToRay(Input.mousePosition);
#else
        ray = cam.ScreenPointToRay(middleScreen);
#endif
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 200f, layerMask))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "ToKillAgent":
                    if (hit.transform.gameObject.tag.Contains("ToKillAgent"))
                    {
                        if (hit.distance < 2.0f)
                        {
                            
                            if (IsSomeoneWatching())
                            {
                                if (gameManager.IsTutorial())
                                {
                                    gameManager.ReplacePlayerOnStartPosition();
                                }
                                else
                                {
                                    gameManager.CinematicPlayerDie();
                                }
                            }
                            else
                            {
                                NavMeshAgentManager.Instance().CopsGoOnCrimeScene();
                                gameManager.KillTarget();
                                if (gameManager.IsTutorial())
                                {
                                    TutorialManager.Instance().NextStep();
                                }
                            }
                        }
                    }
                    break;
                case "Orb":
                    if(hit.transform.parent.CompareTag("Orb"))
                        Destroy(hit.transform.parent.gameObject);
                    else
                        Destroy(hit.transform.gameObject);
                    
                    OrbManager.IncrementOrb(UnityEngine.Random.Range(0f, 1f) < .5f);
                    break;
                
                case "InteractiveElement":
                    Vector3 origin = hit.transform.position;
                    Destroy(hit.transform.gameObject);
                    break;
                case "TargetRoomDoor":
                    if (!gameManager.IsTargetAlive())
                    {
                        CodeMission.Instance().OpenMissionPanel();   
                    }
                    break;
                case "LaptopInfo":
                    PCMission.Instance().OpenMissionPanel();
                    
                    break;
                case "ElevatorDoor":
                    if (!gameManager.IsDataRetrieve())
                    {
                        return;
                    }
                    if (gameManager.IsTutorial())
                    {
                        if (TutorialManager.Instance().GetStep() == 6)
                        {
                            gameManager.EndTutorial();
                            return;
                        }
                    }
                    gameManager.GoToNextFloor(hit.transform.parent.gameObject);
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
        #if UNITY_STANDALONE
        if (canRotate)
        {
            playerBody.RotateAround(playerBody.position, playerBody.up , yRotation * lookSensitivity /** Time.fixedDeltaTime*/);
            transform.localRotation = Quaternion.Euler(xRotation * lookSensitivity /** Time.fixedDeltaTime*/, 0f, 0f);
            
        }
        #else
        if (canRotate)
        {
            playerBody.Rotate(0f, -xRotation , 0f, Space.Self);
            transform.Rotate(yRotation, 0f, 0f, Space.Self);
        }
        xRotation = 0f;
#endif
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

    public void EnableRotation()
    {
        canRotate = true;
    }
    
    public void DisableRotation()
    {
        canRotate = false;
    }
    
    public void EnableClick()
    {
        canClick = true;
    }
    
    public void DisableClick()
    {
        canClick = false;
    }
}
