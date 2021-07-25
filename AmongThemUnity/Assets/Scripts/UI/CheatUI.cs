using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatUI : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text wealthText;
    
    
    private void OnEnable()
    {
        wealthText.text = Math.Round(ProgressionManager.GetWealthValue(), 2).ToString();
    }

    public void AddRedOrb()
    {
        OrbManager.IncrementOrb(true);
    }

    public void AddBlueOrb()
    {
        OrbManager.IncrementOrb(false);
    }

    public void ChangeWealth(bool isAdd)
    {
        if (isAdd)
            ProgressionManager.SetWealthValueShortCut(ProgressionManager.GetWealthValue() + 0.1f);
        else
            ProgressionManager.SetWealthValueShortCut(ProgressionManager.GetWealthValue() - 0.1f);
        
        wealthText.text = Math.Round(ProgressionManager.GetWealthValue(), 2).ToString();
    }
}
