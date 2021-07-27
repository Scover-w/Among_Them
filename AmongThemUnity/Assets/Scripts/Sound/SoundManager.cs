using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private List<Sound> sounds;

    private float globalVolume = 1f;
    
    private Sound hearthSound;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = 1f * globalVolume;
        }
        
    }

    public void SetGlobalVolume(float vol)
    {
        globalVolume = vol;
    }

    public void Play(string name)
    {
        Sound s = sounds.First(ss => ss.Name == name);
        s.source.volume = 1f * globalVolume;
        s.source.Play();
    }
    
    public void PlayMusic(string name)
    {
        Sound s = sounds.First(ss => ss.Name == name);
        s.source.volume = 0.2f * globalVolume;
        s.source.loop = true;
        s.source.Play();
    }
    
    public void Stop(string name)
    {
        Sound s = sounds.First(ss => ss.Name == name);

        s.source.Stop();
    }

    public void PlayHearth()
    {
        hearthSound = sounds.First(ss => ss.Name == "Hearth");
        hearthSound.source.loop = true;
        hearthSound.source.volume = 0f;
        hearthSound.source.Play();
    }

    
    public void Click()
    {
        sounds.First(ss => ss.Name == "Click").source.Play();
    }
    
    public void StopHearth()
    {
        if(hearthSound != null)
            hearthSound.source.Stop();
    }

    public void SetVolumeHearth(float volume)
    {
        if(hearthSound != null)
            hearthSound.source.volume = volume * globalVolume;
    }
}
