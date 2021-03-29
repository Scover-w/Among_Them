using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCount : MonoBehaviour
{
    [SerializeField] 
    private Text fps;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        fps.text = (1/Time.deltaTime).ToString();
    }
}
