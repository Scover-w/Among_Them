using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanelUI : MonoBehaviour
{
    public InputField codeDisplay;
    public string number;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToDisplay()
    {
        if (codeDisplay.text.Length < 4)
        {
            codeDisplay.text += number;
        }
    }

    public void RemoveFromDisplay()
    {
        if (codeDisplay.text.Length > 0)
        {
            codeDisplay.text = codeDisplay.text.Remove(codeDisplay.text.Length-1);
        }
    }

    public void ConfirmCode()
    {
        if (codeDisplay.text.Equals(CodeMission.Instance().GetCode()))
        {
            codeDisplay.text = "TRUE";
            return;
        }
        codeDisplay.text = "FALSE";
    }
}
