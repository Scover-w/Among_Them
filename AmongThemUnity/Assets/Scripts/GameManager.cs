using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    private bool isTutorial;
    private Vector3 startPosition;
    
    private string[] names = new []{"Sanchez", "Hernandez", "Rodriguez", "Fernandez", "Santiago"};
    private string[] surnames = new []{"Pedro", "Manuel", "Miguel", "Javier", "Angel"};

    [SerializeField]
    private Text targetName;
    
    private GameObject target;
    [SerializeField]
    private GameObject player;
    
    [SerializeField]
    private Text codeText;
    
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject fpsCanvas;

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
        targetName.text = $"{surnames[Random.Range(0, surnames.Length - 1)]}  {names[Random.Range(0, names.Length - 1)]}";
        target = NavMeshAgentManager.Instance().GetTargetAgent();
        Debug.Log(player.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            DeleteEverythingExcept.Instance().DestroyObject();
            DeleteEverythingExcept.Instance().DoNotDestroyObject();
        }
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
        Time.timeScale = 0;
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
        playerCanRotate = canClick;
    }

    public void KillTarget()
    {
        var code = CodeMission.Instance().RandomizeCode();
        codeText.text = "Code : " + code;
        targetIsAlive = false;
        target.SetActive(false);
    }

    public void StartTutorial()
    {
        isTutorial = true;
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
        targetName.text = $"{surnames[Random.Range(0, surnames.Length - 1)]}  {names[Random.Range(0, names.Length - 1)]}";
    }

    public void GoToNextFloor()
    {
        //
    }
}
