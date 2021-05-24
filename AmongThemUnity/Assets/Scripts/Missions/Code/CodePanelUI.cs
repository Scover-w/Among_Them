using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanelUI : MonoBehaviour
{
    
    public InputField codeDisplay;
    
    public void RemoveFromDisplay()
    {
        if (codeDisplay.text.Length > 0)
        {
            codeDisplay.text = codeDisplay.text.Remove(codeDisplay.text.Length-1);
        }
    }

    public void ConfirmCode()
    {
        CodeMission.Instance().ConfirmCode(codeDisplay);
    }
    
}
