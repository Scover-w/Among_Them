using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance()
    {
        return _singleton;
        
    }
    private static GameManager _singleton;

    private bool isGamePaused;
    private bool playerCanMove;
    private bool playerCanRotate;
    private bool playerCanClick;
    private bool targetIsAlive;
    private bool dataRetrieve;
    private bool isTutorial;
    private Vector3 startPosition;
    
    private string[] names = new []{"Sanchez", "Hernandez", "Rodriguez", "Fernandez", "Santiago"};
    private string[] surnames = new []{"Pedro", "Manuel", "Miguel", "Javier", "Angel"};

    [Header("Target and Code")] 
    [SerializeField]
    private GameObject parentTarget;
    [SerializeField]
    private GameObject parentCode;
    [SerializeField]
    private TMP_Text targetName;
    [SerializeField]
    private TMP_Text codeText;
    
    
    private GameObject target;
    private GameObject appartmentTargetDoor;
    [SerializeField] private Material glowingObjectMat;
    [SerializeField]
    private GameObject player;
    
    
    
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject fpsCanvas;

    private int floor;

    private void Awake()
    {
        _singleton = this;
        isGamePaused = false;
        pauseMenu.SetActive(false);
        fpsCanvas.SetActive(true);
        playerCanMove = true;
        playerCanRotate = true;
        playerCanClick = true;
        targetIsAlive = true;
        isTutorial = false;
        startPosition = player.transform.position;
    }

    void Start()
    {
        floor = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        targetName.text = $"{surnames[Random.Range(0, surnames.Length - 1)]}  {names[Random.Range(0, names.Length - 1)]}";
        parentTarget.SetActive(true);
        parentCode.SetActive(false);
        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        yield return null;
        yield return null;
        if (!isTutorial)
        {
            GenerateNewMap();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePlayerCanClick(false);
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GenerateNewMap();
        }
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
    }


    public bool PauseGame()
    {
        if (isGamePaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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
        var code = CodeMission.Instance().RandomizeCode();
        codeText.text = "Code : " + code;
        parentTarget.SetActive(false);
        parentCode.SetActive(true);
        targetIsAlive = false;
        target.SetActive(false);
        appartmentTargetDoor.GetComponent<MeshRenderer>().material = glowingObjectMat;
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
        dataRetrieve = true;
        targetName.text = $"{surnames[Random.Range(0, surnames.Length - 1)]}  {names[Random.Range(0, names.Length - 1)]}";
        parentTarget.SetActive(true);
        parentCode.SetActive(false);
    }

    public void GoToNextFloor()
    {
        dataRetrieve = false;
        floor++;
        if (floor == 5)
        {
            EndGame();
        }
        GenerateNewMap();
        ReplacePlayerOnStartPosition();
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

    public void EndGame()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
