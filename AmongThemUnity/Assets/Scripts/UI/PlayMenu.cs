﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    public void Play()
    {
        /*if (!PlayerPrefs.HasKey("alradyStart"))
        {
            PlayerPrefs.SetInt("alreadyStart",1);
            SceneManager.LoadScene("CrowdGameplay", LoadSceneMode.Single);
        }*/
        SceneManager.LoadScene("RandomGamePlay", LoadSceneMode.Single);
    }
}
