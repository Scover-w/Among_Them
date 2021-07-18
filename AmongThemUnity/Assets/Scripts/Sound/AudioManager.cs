using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider soundSlider;

    public TMP_Text volumeValue;

    private float volume;
    void Start()
    {
        StartCoroutine(nameof(StartCo));
    }

    IEnumerator StartCo()
    {
        yield return null;
        yield return null;
        
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            volume =  PlayerPrefs.GetFloat("soundVolume");
            SoundManager.Instance.SetGlobalVolume(volume);
        }
        else
        {
            volume = 0.8f;
            SoundManager.Instance.SetGlobalVolume(volume);
            PlayerPrefs.SetFloat("soundVolume", volume);
        }
        
        soundSlider.value = volume;
        soundSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }
    
    void ChangeVolume()
    {
        volume = soundSlider.value;
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
        SoundManager.Instance.SetGlobalVolume(volume);
    }

    // Update is called once per frame
    void Update()
    {
        int val = (int) (volume * 100);
        
        volumeValue.text = val.ToString();
    }
}
