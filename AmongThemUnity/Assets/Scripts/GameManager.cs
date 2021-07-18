﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public enum Platform {PC, Android}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance()
    {
        return _singleton;
        
    }
    private static GameManager _singleton;

    //Boolean
    private bool isGamePaused;
    private bool playerCanMove;
    private bool playerCanRotate;
    private bool playerCanClick;
    private bool targetIsAlive;
    private bool dataRetrieve;
    private bool isTutorial;
    private Vector3 startPosition;
    
    //Name List
    private string[] names = new []{"Sanchez", "Hernandez", "Rodriguez", "Fernandez", "Santiago"};
    private string[] surnames = new []{"Pedro", "Manuel", "Miguel", "Javier", "Angel"};

    // Main Camera
    [SerializeField] 
    private Camera mainCamera;
    
    //Text Objectifs
    [Header("Target and Code")] 
    [SerializeField]
    private GameObject parentTarget;
    [SerializeField]
    private GameObject parentCode;
    [SerializeField]
    private TMP_Text targetName;
    [SerializeField]
    private TMP_Text codeText;
    
    //GameObject Objectif
    private GameObject target;
    private GameObject appartmentTargetDoor;
    [SerializeField] private Material glowingObjectMat;
    [SerializeField]
    private GameObject player;
    
    
    //Canvas
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject fpsCanvas;
    [SerializeField]
    private GameObject UICanvas;
    [SerializeField]
    private GameObject gameOverCanvas;

    [SerializeField] 
    private GameObject blackScreen;
    [SerializeField] 
    private GameObject whiteTransition;
    
    //GO Text
    [SerializeField]
    private TMP_Text goText;
    
    //Time
    [SerializeField]
    private TMP_Text endTime;

    private float timeStart;
    private float timeEnd;
    
    //Database
    [SerializeField]
    private TestConnexion DBConnexion;


    [SerializeField] 
    private Transform containerMap;
    
    private Platform platform;

    // Elevator cinematic
    [SerializeField] 
    private DoorElevatorManager elevatorManager;
    [SerializeField] 
    private CamCinematic camCinematic;

    [SerializeField] 
    private GameObject prefabPolyPlayer;
    private GameObject polyPlayer;
    
    // Orb
    [SerializeField] 
    private GameObject orbG;

    private void Awake()
    {
#if UNITY_STANDALONE
        platform = Platform.PC;
#else
        platform = Platform.Android;
#endif
        
        
        _singleton = this;
        isGamePaused = false;
        pauseMenu.SetActive(false);
        fpsCanvas.SetActive(true);
        targetIsAlive = true;
        isTutorial = false;
        startPosition = player.transform.position;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        targetName.text = $"{surnames[Random.Range(0, surnames.Length - 1)]}  {names[Random.Range(0, names.Length - 1)]}";
        parentTarget.SetActive(true);
        parentCode.SetActive(false);
        timeStart = Time.time;
        StartCoroutine(StartGame());
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    IEnumerator StartGame()
    {
        blackScreen.SetActive(true);
        yield return null;
        yield return null;
        OrbManager.GetOrbs();
        
        if (!isTutorial)
        {
            GenerateNewMap();
        }
        else
        {
            StartCoroutine(InstantiateCrowd());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePlayerCanClick(false);
            PauseGame();
        }

        /*if (Input.GetKeyDown(KeyCode.L))
        {
            GenerateNewMap();
        }*/
        
    }

    public void GenerateNewMap()
    {
        foreach(Transform child in ProceduralManager.ParentMap)
        {
            Destroy(child.gameObject);
        }
        ProceduralManager.instance.Shuffle();
        StartCoroutine(InstantiateCrowd());
    }

    IEnumerator InstantiateCrowd()
    {
        yield return null;
        yield return null;
        NavMeshAgentManager.Instance().InstantiateCrowd();
        target = NavMeshAgentManager.Instance().GetTargetAgent();

        StartCoroutine(nameof(BeginLevelCinematic));
    }

    IEnumerator BeginLevelCinematic()
    {
        yield return new WaitForSeconds(15f);
        elevatorManager.TeleportPlayer();
        blackScreen.SetActive(false);
        yield return new WaitForSeconds(2f);
        elevatorManager.OpenElevatorBeginLevel();
        playerCanMove = true;
        playerCanRotate = true;
        playerCanClick = true;
    }

    public void GoToNextFloor(GameObject elevator)
    {
        ProgressionManager.NextLevel();

        targetIsAlive = true;
        playerCanMove = false;
        playerCanRotate = false;
        playerCanClick = false;
        dataRetrieve = false;
        StartCoroutine(nameof(EndLevelCinematic), elevator);
    }
    
    IEnumerator EndLevelCinematic(GameObject elevator)
    {
        elevatorManager.OpenElevatorEndLevel(elevator);
        yield return new WaitForSeconds(1f);
        whiteTransition.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        player.transform.position = elevator.transform.position;
        player.transform.rotation = elevator.transform.rotation;
        player.transform.Rotate(Vector3.up, 180f);
        yield return new WaitForSeconds(0.5f);
        
        yield return new WaitForSeconds(2f);
        elevatorManager.CloseElevatorEndLevel();
        yield return new WaitForSeconds(1f);


        
        player.SetActive(false);
        camCinematic.PlayCinematic();
        yield return new WaitForSeconds(camCinematic.GetCinematicTimer());

        if (Math.Abs(ProgressionManager.GetWealthValue() - 1f) < 0.001f)
        {
            EndGame(true);
        }
        else
        {
            blackScreen.SetActive(true);
            GenerateNewMap();
        }
    }
    
    public bool PauseGame()
    {
        if (isGamePaused)
        {
            if (platform == Platform.PC)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
            fpsCanvas.SetActive(true);
            return isGamePaused = false;
        }
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fpsCanvas.SetActive(false);
        return isGamePaused = true;
    }

    public bool PlayerCanMove => playerCanMove;

    public bool PlayerCanRotate => playerCanRotate;
    public bool PlayerCanClick => playerCanClick;
    public bool IsTutorial => isTutorial;

    public bool TargetIsAlive => targetIsAlive;

    public bool DataRetrieve => dataRetrieve;

    public void ChangePlayerCanRotate(bool canRotate)
    {
        playerCanRotate = canRotate;
    }
    
    public void ChangePlayerCanMove(bool canMove)
    {
        playerCanRotate = canMove;
    }
    
    public void ChangePlayerCanClick(bool canClick)
    {
        playerCanClick = canClick;
    }

    public void KillTarget()
    {
        SoundManager.Instance.Play("Kill");
        var code = CodeMission.Instance().RandomizeCode();
        codeText.text = "Code : " + code;
        parentTarget.SetActive(false);
        parentCode.SetActive(true);
        targetIsAlive = false;
        target.SetActive(false);
        if (!isTutorial)
        {
            appartmentTargetDoor.GetComponent<MeshRenderer>().material = glowingObjectMat;
        }
        
        SoundManager.Instance.PlayHearth();
        SoundManager.Instance.PlayMusic("SadMusic");
        StartCoroutine(nameof(HearthBeat));
    }

    public void StartTutorial()
    {
        isTutorial = true;
        NavMeshAgentManager.Instance().InstantiateCrowd();
    }
    
    public void EndTutorial()
    {
        isTutorial = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void ReplacePlayerOnStartPosition()
    {
        player.transform.position = startPosition;
    }
    
    public void GetNextTargetInformation()
    {
        SoundManager.Instance.Play("Poi");
        dataRetrieve = true;
        targetName.text = $"{surnames[Random.Range(0, surnames.Length - 1)]}  {names[Random.Range(0, names.Length - 1)]}";
        parentTarget.SetActive(true);
        parentCode.SetActive(false);
    }

    public void AppartmentTargetDoor(GameObject appartmentTargetDoor)
    {
        this.appartmentTargetDoor = appartmentTargetDoor;
    }

    public void OpenDoor()
    {
        appartmentTargetDoor.SetActive(false);
        //appartmentTargetDoor.transform.position = appartmentTargetDoor.transform.position + Vector3.left;
    }

    public void EndGame(bool win)
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fpsCanvas.SetActive(false);
        UICanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        goText.text = "Lose";
        endTime.text = "-";
        if (win)
        {
            goText.text = "Win";
            timeEnd = Time.time;
            string finalTime = ConvertTimeToString(timeEnd - timeStart);
            endTime.text = finalTime;
            StartCoroutine(DBConnexion.SendData(finalTime, 1));
        }
        else
        {
            SoundManager.Instance.Play("Lose");
        }
        
        //SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public string ConvertTimeToString(float time)
    {
        int timeTemp = (int)time;
        int hours = (timeTemp / 3600);
        timeTemp -= 3600 * hours;
        
        int min = timeTemp / 60;
        timeTemp -= 60 * min;


        string hText = hours.ToString();
        string mText = min.ToString();
        string sText = timeTemp.ToString();
        
        if (hours < 10)
        {
            hText = "0" + hours;
        }
        
        if (min < 10)
        {
            mText = "0" + min;
        }
        
        if (timeTemp < 10)
        {
            sText = "0" + timeTemp;
        }
        
        string timeString = hText + ":" + mText + ":" + sText;

        return timeString;
    }

    public void StopHearthBeat()
    {
        SoundManager.Instance.Stop("SadMusic");
        StopCoroutine(nameof(HearthBeat));
    }
    
    IEnumerator HearthBeat()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        Vector2 distanceDoor;
        while (true)
        {
            yield return wait;
            distanceDoor.x = player.transform.position.x - appartmentTargetDoor.transform.position.x;
            distanceDoor.y = player.transform.position.z - appartmentTargetDoor.transform.position.z;
            if (distanceDoor.magnitude < 50f)
            {
                SoundManager.Instance.SetVolumeHearth(1f - distanceDoor.magnitude / 50f);
            }
            else
            {
                SoundManager.Instance.SetVolumeHearth(0f);
            }
        }
    }

    public void UseOrb(bool isRed)
    {
        if (!PlayerCanClick)
        {
            return; 
        }

        if (isRed && OrbManager.GetRedCount() < 1)
            return;
        
        if (!isRed && OrbManager.GetBlueCount() < 1)
            return;

        Vector3 position = RaycastMesh();
        if (position == Vector3.zero)
            return;
        OrbManager.DecrementOrb(isRed);
        GameObject orb = Instantiate(orbG, containerMap);
        orb.transform.position = position + new Vector3(0f, 1f, 0f);
        orb.GetComponent<OrbBehaviour>().StartTicTac(isRed);
    }

    public Vector3 RaycastMesh()
    {
        int layerMask = 1 << 8;
        
        layerMask = ~layerMask;
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f, layerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
