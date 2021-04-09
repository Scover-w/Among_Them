using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance()
    {
        return _singleton;
        
    }
    private static GameManager _singleton;

    private bool isGamePaused;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject fpsCanvas;
    
    void Start()
    {
        _singleton = this;
        isGamePaused = false;
        pauseMenu.SetActive(false);
        fpsCanvas.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
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
}
