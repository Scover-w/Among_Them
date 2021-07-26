﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        
        if (!PlayerPrefs.HasKey("selectedLanguage"))
        {
            if(Application.systemLanguage == SystemLanguage.French)
            {
                PlayerPrefs.SetString("selectedLanguage", "fr");
            }
            else
            {
                PlayerPrefs.SetString("selectedLanguage", defaultLanguage);
            }
        }
        
        selectedLanguage = PlayerPrefs.GetString("selectedLanguage");
        
        languages = Initialization("Language/lang");
 
    }

    void Start()
    {
        ctScript = GetComponent<ChangeText>();

    }

    Dictionary<String, Dictionary<string, string>> Initialization(string path)
    {
        TextAsset textAsset = Resources.Load(path) as TextAsset;

        XmlTextReader reader = new XmlTextReader(new StringReader(textAsset.text));
        
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
        if (lang.Length == 2)
            return lang;
        switch (lang.ToLower())
        {
            case "french":
                return "fr";
            case "france":
                return "fr";
            case "français":
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

        PlayerPrefs.SetString("selectedLanguage", GetCodeLanguageFromFullName(lang));
        
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
