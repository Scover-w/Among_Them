using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ReturnToGame()
    {
        Debug.Log("Play");
        GameManager.Instance().PauseGame();
    }
    
    public void Settings()
    {
        GameManager.Instance().PauseGame();
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
