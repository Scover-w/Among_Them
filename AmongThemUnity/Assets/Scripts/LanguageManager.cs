using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance() { return _singleton; }
    private static LanguageManager _singleton;

    private Dictionary<String, Dictionary<string, string>> languages;

    private string selectedLanguage = "fr";

    private const string defaultLanguage = "en";
    
    [SerializeField]
    private ReferenceAndText[] texts;

    private ChangeText ctScript;

    private void Awake()
    {
        _singleton = this;
        if (selectedLanguage == null)
        {
            selectedLanguage = defaultLanguage;
        }

        string path = Application.dataPath;
        languages = Initialization(path + "/Language/lang.xml");
        Debug.Log(languages[selectedLanguage]["hello"]);
    }

    void Start()
    {
        ctScript = GetComponent<ChangeText>();

    }

    Dictionary<String, Dictionary<string, string>> Initialization(string path)
    {
        XmlReader reader = XmlReader.Create(@path);
        Dictionary<String, Dictionary<string, string>> temp = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> insideTemp = new Dictionary<string, string>();
        string lang = null;
        while (reader.Read())
        {
            if (reader.IsStartElement())
            {
                switch (reader.Name)
                { 
                        case "string":
                            insideTemp.Add(reader.GetAttribute("name"), reader.ReadString());
                            break;
                        default:
                            if (reader.HasAttributes)
                            {
                                if (lang != null)
                                {
                                    temp.Add(lang, insideTemp);
                                }

                                lang = reader.GetAttribute("val");
                                insideTemp = new Dictionary<string, string>();
                            }
                            break;
                }
            }
        }

        return temp;
    }

    public string GetTextWithReference(string reference)
    {
        
        var text = languages[selectedLanguage].ContainsKey(reference)? languages[selectedLanguage][reference] : "Text not found";
        return text;
    }

    public string GetCodeLanguageFromFullName(string lang)
    {
        switch (lang.ToLower())
        {
            case "french":
                return "fr";
            case "france":
                return "fr";
            case "english" :
                return "en";
            case "anglais":
                return "en";
            default:
                return "en";
        }
    }

    public void ChangeSelectedLanguage(string lang)
    {
        selectedLanguage = lang;
        ctScript.ChangeTexts();
    }

    public string GetSelectedLanguage()
    {
        return selectedLanguage;
    }

    public int GetPositionSelectedLanguage()
    {
        switch (selectedLanguage)
        {
            case "fr":
                return 1;
            case "en" :
                return 0;
            default:
                return 0;
        }
    }
}
