using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource al;

    public Slider soundSlider;

    public TMP_Text volumeValue;

    private float volume;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            volume =  PlayerPrefs.GetFloat("soundVolume");
            al.volume = volume;
        }
        else
        {
            volume = al.volume;
            PlayerPrefs.SetFloat("soundVolume", volume);
        }
        
        soundSlider.value = volume;
        soundSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        
    }

    void ChangeVolume()
    {
        volume = soundSlider.value;
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
        al.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        int val = (int) (volume * 100);
        volumeValue.text = val.ToString();
    }
}
