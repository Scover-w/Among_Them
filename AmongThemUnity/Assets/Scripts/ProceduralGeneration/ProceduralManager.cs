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
    private ProceduralStair proceduralStair;
    
    [SerializeField]
    private ProceduralStorey proceduralStorey;

    [Range(0f,1f)]
    public float wealthValue = 0f;
    
    
    public List<ObstructedLocation> obstructedLocation;
    
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
        obstructedLocation = new List<ObstructedLocation>();
            
        obstructedLocation = obstructedLocation.Union(proceduralElevator.LoadElevators(wealthLevel)).ToList();
        
        obstructedLocation = obstructedLocation.Union(proceduralStorey.LoadStoreys(obstructedLocation, wealthLevel)).ToList();
        
        obstructedLocation = obstructedLocation.Union(proceduralStair.LoadStairs(obstructedLocation, wealthLevel)).ToList();

    }
}
