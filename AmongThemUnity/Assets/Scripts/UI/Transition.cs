using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] 
    private RectTransform whiteTransition;

    [SerializeField] 
    private GameObject thisGameObject;
    
    private float timeTransition = 1f;
    // Start is called before the first frame update
    private void OnEnable()
    {
        whiteTransition.position = new Vector3(-1500f, 0f, 0f);
        StartCoroutine(nameof(Transit));
    }

    IEnumerator Transit()
    {
        float timer = 0f;
        float deltaX = 3000f / timeTransition * 2;
        while (timer < timeTransition)
        {
            yield return null;
            whiteTransition.Translate(deltaX * Time.deltaTime, 0f, 0f, Space.World);
            timer += Time.deltaTime;
            
        }

        thisGameObject.SetActive(false);
    }
}
