using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetection : MonoBehaviour
{

    public Image eyeVisionOpen;
    private int isVisible;
    private int copsVisible;

    private void Start()
    {
        isVisible = 0;
        eyeVisionOpen.gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("FieldViewAgent"))
        {
            isVisible++;
            eyeVisionOpen.gameObject.SetActive(true);
        }
        
        if (other.tag.Equals("FieldViewCops"))
        {
            isVisible++;
            copsVisible++;
            eyeVisionOpen.gameObject.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("FieldViewAgent"))
        {
            isVisible--;
        }
        
        if (other.tag.Equals("FieldViewCops"))
        {
            isVisible--;
            copsVisible--;
        }
        

        if (isVisible == 0)
        {
            eyeVisionOpen.gameObject.SetActive(false);
        }
    }

    public bool IsPlayerVisible()
    {
        if (isVisible > 0)
        {
            return true;
        }

        return false;
    }

    public bool CopsWatchingYou()
    {
        if (copsVisible > 0)
        {
            return true;
        }

        return false;
    }
}
