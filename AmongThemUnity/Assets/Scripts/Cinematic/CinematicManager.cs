using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] 
    private SoundManager soundManager;
    void Start()
    {
        StartCoroutine(nameof(WaitEndVideo));

        StartCoroutine(nameof(StartVoice));
    }
    
    IEnumerator WaitEndVideo()
    {
        yield return new WaitForSeconds(113f); // 113f
        SceneManager.LoadScene("RandomGamePlay", LoadSceneMode.Single);
    }

    IEnumerator StartVoice()
    {
        yield return null;
        string language = LanguageManager.Instance().GetSelectedLanguage();
        if (language == "fr")
        {
            soundManager.Play("FrenchVoice");
        }
        else
        {
            soundManager.Play("EnglishVoice");
        }
    }

    public void SkipVideo()
    {
        string language = LanguageManager.Instance().GetSelectedLanguage();
        if (language == "fr")
        {
            soundManager.Stop("FrenchVoice");
        }
        else
        {
            soundManager.Stop("EnglishVoice");
        }
        SceneManager.LoadScene("RandomGamePlay", LoadSceneMode.Single);
    }
}
