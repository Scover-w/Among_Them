using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObstructedLocation
{
    Middle,
    Short,
    Long
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
    
    public bool Shuffle = true;
    private bool tempShuffle = true;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Shuffle != tempShuffle)
        {
            LoadBuilding(wealthValue);
            tempShuffle = Shuffle;
        }
    }

    public void LoadBuilding(float wealthLevel)
    {
        var obstructedLocation = new List<ObstructedLocation>();
            
        obstructedLocation = obstructedLocation.Union(proceduralElevator.LoadElevators(wealthLevel)).ToList();

    }
}
