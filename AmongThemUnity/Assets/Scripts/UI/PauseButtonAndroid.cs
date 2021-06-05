using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonAndroid : MonoBehaviour
{
    public void ClickPauseGame()
    {
        GameManager.Instance().PauseGame();
    }
}
