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

    private bool isRedSelected = false;
    private void Start()
    {
        instance = this;
    }

    public bool GetSelectedOrb()
    {
        return isRedSelected;
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

        if ((redOrb == 0 && blueOrb != 0) || (redOrb != 0 && blueOrb == 0))
        {
            if (redOrb == 0)
            {
                blueSelect.SetActive(true);
                redSelect.SetActive(false);
            }
            else
            {
                redSelect.SetActive(true);
                blueSelect.SetActive(false);
            }
                
        }
        
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
        GameManager.Instance().UseOrb(true);
    }

    public void SelectOrb(int scroll)
    {
        if (scroll % 2 == 0)
            return;

        isRedSelected = !isRedSelected;

        redSelect.SetActive(isRedSelected);
        blueSelect.SetActive(!isRedSelected);
            
    }
}
