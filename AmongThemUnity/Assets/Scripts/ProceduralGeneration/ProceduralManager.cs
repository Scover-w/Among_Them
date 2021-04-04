using System.Collections;
using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        LoadBuilding(1f);
    }

    public void LoadBuilding(float wealthLevel)
    {
        List<int> obstructedLocation = new List<int>();
        
        proceduralElevator.LoadElevators(wealthLevel);
    }
}
