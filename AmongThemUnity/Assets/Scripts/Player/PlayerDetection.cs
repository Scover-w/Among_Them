using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetection : MonoBehaviour
{

    public Image eyeVisionOpen;
    private int isVisible;

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("FieldViewAgent"))
        {
            isVisible--;
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
}
