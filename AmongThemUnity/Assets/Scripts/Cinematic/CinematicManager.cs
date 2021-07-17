using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(WaitEndVideo));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitEndVideo()
    {
        yield return new WaitForSeconds(113f);
        Debug.Log("EndVideo");
    }
}
