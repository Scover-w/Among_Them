﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveIfAndroid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        #if UNITY_STANDALONE || UNITY_EDITOR
        gameObject.SetActive(false);
        #endif
    }


}
