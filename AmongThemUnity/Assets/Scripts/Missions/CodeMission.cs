using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeMission : MonoBehaviour
{
    public static CodeMission Instance() {return _singleton;}
    private static CodeMission _singleton;

    private string code = "0000";
    void Start()
    {
        _singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetCode()
    {
        return code;
    }
}
