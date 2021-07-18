using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingLogo : MonoBehaviour
{
    [SerializeField] 
    private RectTransform barbele;
    
    [SerializeField] 
    private RectTransform pelican;

    private float time = 0f;
    private float scale;
    private void OnEnable()
    {
        barbele.rotation = Quaternion.identity;
        pelican.localScale = Vector3.one;
        time = 0f;
    }

    private void Update()
    {
        time += Time.deltaTime;
        barbele.Rotate(0f, 0f, 20f * Time.deltaTime);
        scale = (Mathf.Sin(time) + 9f) / 10f;
        pelican.localScale = new Vector3(scale, scale, scale);
        
    }
}
