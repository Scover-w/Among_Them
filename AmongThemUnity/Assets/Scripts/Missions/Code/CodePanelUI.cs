using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CodePanelUI : MonoBehaviour
{
    
    public TMP_InputField codeDisplay;
    
    public void RemoveFromDisplay()
    {
        if (codeDisplay.text.Length > 0)
        {
            codeDisplay.text = codeDisplay.text.Remove(codeDisplay.text.Length-1);
        }
    }

    public void ClickButton()
    {
        SoundManager.Instance.Click();
    }
    public void ConfirmCode()
    {
        CodeMission.Instance().ConfirmCode(codeDisplay);
    }
    
}
