using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string Name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector] 
    public AudioSource source;
}
