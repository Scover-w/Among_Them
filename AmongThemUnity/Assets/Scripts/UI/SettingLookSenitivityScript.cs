using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingLookSenitivityScript : MonoBehaviour
{
    [SerializeField] 
    private PlayerLook playerLook;

    [SerializeField] 
    private Slider sliderLS;
    
    [SerializeField] 
    private Text valueSensitivity;
    
    // Start is called before the first frame update
    void Start()
    {
        sliderLS.value = playerLook.GetSensitivity();
        valueSensitivity.text = playerLook.GetSensitivity().ToString();
        sliderLS.onValueChanged.AddListener(delegate { ChangeSensitivity(); });
    }

    void ChangeSensitivity()
    {
        playerLook.SetSensitivity(sliderLS.value);
    }

    private void Update()
    {
        valueSensitivity.text = playerLook.GetSensitivity().ToString();
    }
}
