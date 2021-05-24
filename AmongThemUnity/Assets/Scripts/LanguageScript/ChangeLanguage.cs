using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    public Dropdown ddLang;
    void Start()
    {
        ddLang.onValueChanged.AddListener(delegate { ChangeGameLanguage(LanguageManager.Instance().GetCodeLanguageFromFullName(ddLang.options[ddLang.value].text)); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGameLanguage(string lang)
    {
        LanguageManager.Instance().ChangeSelectedLanguage(lang);
        Debug.Log(LanguageManager.Instance().GetSelectedLanguage());
    }
}
