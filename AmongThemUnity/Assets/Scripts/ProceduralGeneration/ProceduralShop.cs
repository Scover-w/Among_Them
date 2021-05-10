using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralShop : MonoBehaviour
{

    [Header("Shops")] 
    [SerializeField] 
    private GameObject[] poorShop;
    
    [SerializeField] 
    private GameObject[] normalShop;
    
    [SerializeField] 
    private GameObject[] richShop;
    
    
    public void LoadShops(List<ObstructedLocation> obstructedLocations, float wealthLevel)
    {
        
    }
}
