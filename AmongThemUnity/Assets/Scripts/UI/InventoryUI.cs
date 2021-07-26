using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [HideInInspector] 
    public static InventoryUI instance;
    
    [SerializeField] 
    private GameObject redGameObject;
    
    [SerializeField] 
    private GameObject blueGameObject;

    [SerializeField]
    private TMP_Text redCountText;
    
    [SerializeField] 
    private TMP_Text blueCountText;

    [SerializeField] 
    private Image background;

    [SerializeField] 
    private GameObject redSelect;
    
    [SerializeField] 
    private GameObject blueSelect;

    private int selectedOrb = 0;

    private bool isAndroid = false;
    private void Start()
    {
        instance = this;
        
        redSelect.SetActive(false);
        blueSelect.SetActive(false);
#if !UNITY_STANDALONE
        isAndroid = true;
#endif
    }

    public int GetSelectedOrb()
    {
        return selectedOrb;
    }
    
    public void DisplayOrb(int redOrb, int blueOrb)
    {
        if (redOrb == 0 && blueOrb == 0)
        {
            background.enabled = false;
            redGameObject.SetActive(false);
            blueGameObject.SetActive(false);
            return;
        }

        
        redSelect.SetActive(selectedOrb == 1 && redOrb > 0);
        blueSelect.SetActive(selectedOrb == 2 && blueOrb > 0);
        
        SetRedOrb(redOrb);
        SetBlueOrb(blueOrb);
    }

    public void SetRedOrb(int redOrb)
    {
        if (redOrb == 0)
            redGameObject.SetActive(false);
        else
        {
            background.enabled = true;
            redCountText.text = redOrb.ToString();
            redGameObject.SetActive(true);
        }
    }
    
    public void SetBlueOrb(int blueOrb)
    {
        if(blueOrb == 0)
            blueGameObject.SetActive(false);
        else
        {
            background.enabled = true;
            blueCountText.text = blueOrb.ToString();
            blueGameObject.SetActive(true);
        }
    }

    public void UseRedOrb()
    {
        GameManager.Instance().UseOrb(true);
    }
    
    public void UseBlueOrb()
    {
        GameManager.Instance().UseOrb(false);
    }

    public void SelectOrb(int scroll)
    {
        selectedOrb += scroll;
        selectedOrb = selectedOrb % 3;
        
        if (!isAndroid)
        {
            redSelect.SetActive(selectedOrb == 1);
            blueSelect.SetActive(selectedOrb == 2);
        }
    }
}
