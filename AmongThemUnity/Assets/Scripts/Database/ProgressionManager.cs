using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ProgressionManager
{
    private static float wealthValue = 0f;
    
    
    public static void EnterGame()
    {
        if (!PlayerPrefs.HasKey("wealthValue"))
        {
            PlayerPrefs.SetFloat("wealthValue", 0f);
        }
        
        wealthValue = PlayerPrefs.GetFloat("wealthValue");

        if (wealthValue == 0f)
        {
            SceneManager.LoadScene("Cinematic", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("RandomGamePlay", LoadSceneMode.Single);
        }
    }

    public static void NextLevel()
    {
        wealthValue += 0.05f;
        PlayerPrefs.SetFloat("wealthValue", wealthValue);
    }

    public static float GetWealthValue()
    {
        return wealthValue;
    }
    
    public static void SetWealthValueShortCut(float value)
    {
        wealthValue = value;
        PlayerPrefs.SetFloat("wealthValue", wealthValue);
    }

}
