using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject panelSettings;

    private void Start()
    {
        panelSettings.SetActive(false);
    }

    public void ReturnToGame()
    {
        GameManager.Instance().ChangePlayerCanClick(true);
        GameManager.Instance().PauseGame();
    }
    
    public void OpenSettings()
    {
        SoundManager.Instance.Click();
        panelSettings.SetActive(true);
    }
    public void CloseSettings()
    {
        SoundManager.Instance.Click();
        panelSettings.SetActive(false);
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
