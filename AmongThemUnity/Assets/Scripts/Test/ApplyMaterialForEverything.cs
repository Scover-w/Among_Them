using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMaterialForEverything : MonoBehaviour
{
    [SerializeField] private Material mat;

    public bool ChangeMaterial = true;
    private bool OldChangeMaterial = false;
    private void Update()
    {
        if (ChangeMaterial == OldChangeMaterial)
        {
            OldChangeMaterial = !ChangeMaterial;
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            MeshRenderer meshRenderer;
            foreach (var gObject in allObjects)
            {
                meshRenderer = gObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.materials = new[] { mat};
                }
                    
            }
        }
    }
}
