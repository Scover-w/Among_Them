using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatActivator : MonoBehaviour
{

    [SerializeField] 
    private GameObject cheatCanvas;

    private List<bool> secretCode;

    private void Start()
    {
        secretCode = new List<bool>();
        secretCode.Add(false);
        secretCode.Add(false);
        secretCode.Add(false);
        secretCode.Add(false);
    }

    public void ButtonClick(bool isLeft)
    {
        secretCode.Add(isLeft);
        secretCode.RemoveAt(0);
        cheatCanvas.SetActive(secretCode[0] && !secretCode[1] && secretCode[2] && !secretCode[3]);
    }
}
