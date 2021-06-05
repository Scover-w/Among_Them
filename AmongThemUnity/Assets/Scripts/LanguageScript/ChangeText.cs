using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct ReferenceAndText
{
    public string reference;
    public TMP_Text text;
}

public class ChangeText : MonoBehaviour
{
    [SerializeField]
    private ReferenceAndText[] texts;
    
    // Start is called before the first frame update
    void Start()
    {
        ChangeTexts();
    }
    
    public void ChangeTexts()
    {
        foreach (var t in texts)
        {
            t.text.text = LanguageManager.Instance().GetTextWithReference(t.reference);
        }
    }
}
