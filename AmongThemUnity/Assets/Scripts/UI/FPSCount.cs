using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSCount : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text fps;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        fps.text = Math.Round(1f/Time.deltaTime).ToString();
    }
}
