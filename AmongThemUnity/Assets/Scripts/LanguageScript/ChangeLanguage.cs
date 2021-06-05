using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeLanguage : MonoBehaviour
{
    public TMP_Dropdown ddLang;
    void Start()
    {
        ddLang.value = LanguageManager.Instance().GetPositionSelectedLanguage();
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
