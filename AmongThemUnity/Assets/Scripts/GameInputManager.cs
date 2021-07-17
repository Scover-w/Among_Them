using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInputManager
{
    private static Dictionary<string, KeyCode> mapping;
    static string[] keyMaps = new string[7]
    {
        "MouseRight",
        "MouseLeft",
        "Forward",
        "Backward",
        "Left",
        "Right",
        "Shift",
    };
    static KeyCode[] defaults = new KeyCode[7]
    {
        KeyCode.Q,
        KeyCode.E,
        KeyCode.Z,
        KeyCode.S,
        KeyCode.Q,
        KeyCode.D,
        KeyCode.LeftShift
    };
 
    static GameInputManager()
    {
        InitializeDictionary();
    }
 
    private static void InitializeDictionary()
    {
        mapping = new Dictionary<string, KeyCode>();
        for(int i=0;i<keyMaps.Length;++i)
        {
            mapping.Add(keyMaps[i], defaults[i]);
        }
    }
    
    public static void SetKeyMap(string keyMap,KeyCode key)
    {
        if (!mapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        mapping[keyMap] = key;
    }
 
    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(mapping[keyMap]);
    }
    
    public static bool GetKey(string keyMap)
    {
        return Input.GetKey(mapping[keyMap]);
    }

    public static KeyCode GetKeyMapOn(string keyMap)
    {
        return mapping[keyMap];
    }
}
