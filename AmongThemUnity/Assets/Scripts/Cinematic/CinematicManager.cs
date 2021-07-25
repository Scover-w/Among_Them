using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(WaitEndVideo));
    }
    
    IEnumerator WaitEndVideo()
    {
        yield return new WaitForSeconds(113f); // 113f
        SceneManager.LoadScene("RandomGamePlay", LoadSceneMode.Single);
    }

    public void SkipVideo()
    {
        SceneManager.LoadScene("RandomGamePlay", LoadSceneMode.Single);
    }
}
