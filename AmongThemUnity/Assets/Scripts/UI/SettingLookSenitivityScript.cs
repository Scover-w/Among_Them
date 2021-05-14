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

    private float sensitivity;
    
    // Start is called before the first frame update
    void Start()
    {
        sensitivity = 50f;
        if (PlayerPrefs.HasKey("lookSensity"))
        {
            sensitivity = PlayerPrefs.GetFloat("lookSensity");
        }
        sliderLS.value = sensitivity;
        sliderLS.onValueChanged.AddListener(delegate { ChangeSensitivity(); });
        if (playerLook)
        {
            playerLook.SetSensitivity(sensitivity);
            valueSensitivity.text = playerLook.GetSensitivity().ToString();
        }
    }

    void ChangeSensitivity()
    {
        sensitivity = sliderLS.value;
        if (playerLook)
        {
            playerLook.SetSensitivity(sliderLS.value);
        }
        PlayerPrefs.SetFloat("lookSensity", sliderLS.value);
    }

    private void Update()
    {
        valueSensitivity.text = sensitivity.ToString();
    }
}
