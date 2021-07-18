using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OrbManager
{
    private static int redOrb;
    private static int blueOrb;

    public static void GetOrbs()
    {
        if (!PlayerPrefs.HasKey("redOrb"))
        {
            PlayerPrefs.SetInt("redOrb", 0);
        }
        
        if (!PlayerPrefs.HasKey("blueOrb"))
        {
            PlayerPrefs.SetInt("blueOrb", 0);
        }
        
        redOrb = PlayerPrefs.GetInt("redOrb");
        blueOrb = PlayerPrefs.GetInt("blueOrb");
        
        InventoryUI.instance.DisplayOrb(redOrb, blueOrb);
    }

    public static void IncrementOrb(bool isRed)
    {
        if (isRed)
        {
            redOrb++;
            PlayerPrefs.SetInt("redOrb", redOrb);
        }
        else
        {
            blueOrb++;
            PlayerPrefs.SetInt("blueOrb", blueOrb);
        }
        InventoryUI.instance.DisplayOrb(redOrb, blueOrb);
    }
    
    public static void DecrementOrb(bool isRed)
    {
        if (isRed)
        {
            redOrb--;
            PlayerPrefs.SetInt("redOrb", redOrb);
        }
        else
        {
            blueOrb--;
            PlayerPrefs.SetInt("blueOrb", blueOrb);
        }
        InventoryUI.instance.DisplayOrb(redOrb, blueOrb);
    }
    
    public static int GetRedCount()
    {
        return redOrb;
    }
    
    public static int GetBlueCount()
    {
        return blueOrb;
    }
}
