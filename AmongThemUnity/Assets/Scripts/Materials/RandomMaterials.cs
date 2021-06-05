using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMaterials : MonoBehaviour
{
    [SerializeField] private MeshRenderer objectMeshRender;

    [SerializeField] private MeshRenderer envMeshRenderer;
    

    public void ApplyRandomColors()
    {
        // HSV color
        float firstColor = Random.Range(0f, 360f);
        float secondColor = (firstColor < 180f) ? firstColor + 180f : firstColor - 180f; // Complementary color
        Debug.Log(firstColor);
        Debug.Log(secondColor);

        Color rgbColor = Color.HSVToRGB(firstColor, 86.7f, 88.2f);
        Debug.Log("rgb color : " + rgbColor.r + " " + rgbColor.g + " " + rgbColor.b);
        objectMeshRender.sharedMaterial.color = Color.HSVToRGB(firstColor / 360f, 86.7f / 100f, 88.2f / 100f);
        
        envMeshRenderer.sharedMaterial.color =  Color.HSVToRGB(secondColor / 360f, 86.7f / 100f, 88.2f / 100f);
    }

}