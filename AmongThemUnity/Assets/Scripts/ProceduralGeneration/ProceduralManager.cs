using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
    
    public NavMeshSurface NavMeshSurface;

    [SerializeField]
    private ProceduralElevator proceduralElevator;

    [SerializeField] 
    private ProceduralShop proceduralShop;
    
    [SerializeField] 
    private ProceduralRoom proceduralRoom;

    [SerializeField] 
    private ProceduralMall proceduralMall;
    
    [SerializeField]
    private ProceduralStair proceduralStair;
    
    [SerializeField]
    private ProceduralStorey proceduralStorey;

    [SerializeField] 
    private RandomMaterials randomMaterials;

    [Range(0f,1f)][Header("Threshold and probability default for each value with two random choices")] 
    public float minThresholdValue = 0.1f;
    
    [Range(0f,1f)]
    public float maxTresholdValue = 0.9f;
    

    [SerializeField] 
    private Transform parentMap;

    public static Transform ParentMap;
    
    private float wealthValue = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        ParentMap = parentMap;
    }

    public void Shuffle()
    {
        wealthValue = ProgressionManager.GetWealthValue();
        
        LoadBuilding(wealthValue);
        randomMaterials.ApplyRandomColors();
        NavMeshSurface.RemoveData();
        StartCoroutine(BuildNavMesh());
    }

    IEnumerator BuildNavMesh()
    {
        yield return null;
        NavMeshSurface.BuildNavMesh();
    }

    public void LoadBuilding(float wealthLevel)
    {
        var obstructedLocation = proceduralElevator.LoadElevators(wealthLevel);

        proceduralShop.LoadShops(obstructedLocation, wealthLevel);

        proceduralRoom.LoadRoom(obstructedLocation, wealthLevel);
        
        obstructedLocation = obstructedLocation.Union(proceduralStorey.LoadStoreys(obstructedLocation, wealthLevel)).ToList();
        
        proceduralMall.LoadMall(obstructedLocation, wealthLevel);
        
        obstructedLocation.Union(proceduralStair.LoadStairs(obstructedLocation, wealthLevel)).ToList();
    }
}
