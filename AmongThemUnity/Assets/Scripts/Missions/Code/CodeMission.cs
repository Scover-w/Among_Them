﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct CodeButton
{
    public Button button;
    public int number;
}
public class CodeMission : MonoBehaviour
{
    public static CodeMission Instance() {return _singleton;}
    private static CodeMission _singleton;

    
    public CodeButton[] buttons;
    public TMP_InputField codeDisplay;
    
    public Canvas missionCanvas;
    public GameObject codeCanvas;

    private string code = "0000";
    void Start()
    {
        _singleton = this;
        code = RandomizeCode();
        foreach (var kvButton in buttons)
        {
            kvButton.button.onClick.AddListener(delegate { AddToDisplay(kvButton.number); });
        }
    }

    public string RandomizeCode()
    {
        List<int> temp = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            temp.Add(Random.Range(0,9));
        }

        string toReturn = String.Join("", temp.ToArray());
        code = toReturn;
        return toReturn;
    }

    public void AddToDisplay(int number)
    {
        if (codeDisplay.text.Length < 4)
        {
            codeDisplay.text += number.ToString();
        }
    }

    public void ConfirmCode(TMP_InputField fieldCode)
    {
        if (fieldCode.text.Equals(code.Substring(0,4)))
        {
            GameManager.Instance().StopHearthBeat();
            SoundManager.Instance.StopHearth();
            SoundManager.Instance.Play("Success");
            fieldCode.text = "TRUE";
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(EndAnimationTrue(fieldCode));
            return;
        }
        
        SoundManager.Instance.Play("Failed");
        fieldCode.text = "FALSE";
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(EndAnimationFalse(fieldCode));
    }

    private IEnumerator EndAnimationTrue(TMP_InputField fieldCode)
    {
        yield return new WaitForSeconds(1);
        missionCanvas.gameObject.SetActive(false);
        fieldCode.text = "";
        CloseMissionPanel();
        if (GameManager.Instance().IsTutorial)
        {
            TutorialManager.Instance().NextStep();
            TutorialManager.Instance().TPinRoom();
        }
        else
        {
            GameManager.Instance().OpenDoor();
        }
    }
    
    private IEnumerator EndAnimationFalse(TMP_InputField fieldCode)
    {
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fieldCode.text = "";
    }

    public void OpenMissionPanel()
    {
        missionCanvas.gameObject.SetActive(true);
        codeCanvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance().ChangePlayerCanMove(false);
        GameManager.Instance().ChangePlayerCanRotate(false);
    }
    
    public void CloseMissionPanel()
    {
        missionCanvas.gameObject.SetActive(false);
        codeCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance().ChangePlayerCanMove(true);
        GameManager.Instance().ChangePlayerCanRotate(true);
    }
}
