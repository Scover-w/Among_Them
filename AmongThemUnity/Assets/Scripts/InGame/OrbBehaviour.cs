using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrbBehaviour : MonoBehaviour
{
    public void StartTicTac(bool isRed)
    {
        StartCoroutine(nameof(TicTac), isRed);
    }
    IEnumerator TicTac(bool isRed)
    {
        yield return new WaitForSeconds((isRed)? 1f : 5f);
        // TO DO : Explode
        SoundManager.Instance.Play("Poi");
        NavMeshAgentManager.Instance().RegroupAround(transform.position);
        Destroy(gameObject);
    }
}
