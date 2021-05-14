using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMission : MonoBehaviour
{
    public static CodeMission Instance() {return _singleton;}
    private static CodeMission _singleton;
    public Canvas missionCanvas;

    private string code = "0000";
    void Start()
    {
        _singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfirmCode(InputField fieldCode)
    {
        if (fieldCode.text.Equals(code))
        {
            fieldCode.text = "TRUE";
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(EndAnimationTrue(fieldCode));
            
            return;
        }
        fieldCode.text = "FALSE";
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(EndAnimationFalse(fieldCode));
    }

    private IEnumerator EndAnimationTrue(InputField fieldCode)
    {
        yield return new WaitForSeconds(1);
        missionCanvas.gameObject.SetActive(false);
        fieldCode.text = "";
    }
    
    private IEnumerator EndAnimationFalse(InputField fieldCode)
    {
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fieldCode.text = "";
    }
}
