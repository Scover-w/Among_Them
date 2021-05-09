using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObstructedLocation
{
    Middle,
    Short,
    Long,
    Corner
}

public class ProceduralManager : MonoBehaviour
{
    public static ProceduralManager instance;

    [SerializeField]
    private ProceduralElevator proceduralElevator;

    [SerializeField] 
    private ProceduralShop proceduralShop;
    
    [SerializeField]
    private ProceduralStair proceduralStair;
    
    [SerializeField]
    private ProceduralStorey proceduralStorey;

    [Range(0f,1f)]
    public float wealthValue = 0f;
    
    
    [Range(0f,1f)][Header("Threshold and probability default for each value with two random choices")] 
    public float minThresholdValue = 0.1f;
    
    [Range(0f,1f)]
    public float maxTresholdValue = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void Shuffle()
    {
        LoadBuilding(wealthValue);
    }

    public void LoadBuilding(float wealthLevel)
    {
        var obstructedLocation = new List<ObstructedLocation>();
            
        obstructedLocation = obstructedLocation.Union(proceduralElevator.LoadElevators(wealthLevel)).ToList();

        proceduralShop.LoadShops(wealthLevel);
        
        obstructedLocation = obstructedLocation.Union(proceduralStorey.LoadStoreys(obstructedLocation, wealthLevel)).ToList();
        
        obstructedLocation = obstructedLocation.Union(proceduralStair.LoadStairs(obstructedLocation, wealthLevel)).ToList();
    }
}
