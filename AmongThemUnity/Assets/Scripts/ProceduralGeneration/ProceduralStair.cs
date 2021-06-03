 using System;
 using System.Collections;
using System.Collections.Generic;
 using System.Linq;
 using UnityEngine;

 public enum DirectionHub
 {
     Inside,
     Outside,
     Null
 }
 public enum WealthStair
 {
     Poor,
     Rich
 }
 public enum LengthStair
 {
     Long, 
     Short,
     Medium
 }
 public enum WidthStair
 {
     Simple,
     Double
 }

 public enum PositionStair
 {
     Short,
     Long,
     Corner
 }

 [Serializable]
 public struct ObstructedPattern
 {
     public PositionStair positionStair;
     public int numberStairs;
     [Range(0.0f, 1.0f)]
     public float wealthValue;
     public List<ObstructedLocation> obstructedLocations;
     

     
 }
 
 [Serializable]
 public struct ObstructedStair
 {
     public GameObject[] simpleStairs;
     public GameObject[] doubleStairs;
     
     public List<ObstructedPattern> ObstructedPatterns;
 }
 
public class ProceduralStair : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] 
    private float SimpleWealthValue;
    
    [SerializeField] [Range(0.0f, 1.0f)] 
    private float DoubleWealthValue;
    
    [SerializeField] 
    private List<ObstructedStair> lengthStairs;
    

    public List<ObstructedLocation> LoadStairs(List<ObstructedLocation> obstructedLocations, float wealthLevel)
    {
        WidthStair choosenWidth = ChooseWidthStair(wealthLevel);
        
        // EdgeCase
        bool isLongCanBeIntegrated = !((obstructedLocations.Contains(ObstructedLocation.Short) &&
                                     obstructedLocations.Contains(ObstructedLocation.Long)) || obstructedLocations.Contains(ObstructedLocation.Corner) );
        
        
        LengthStair choosenLength = ChooseLengthStair(wealthLevel, isLongCanBeIntegrated);
        
        
        WealthStair choosenWealth = ChooseWealthStair(wealthLevel);
        
        
        var idLength = GetIdLength(choosenLength);
        var idWealth = (choosenWealth == WealthStair.Poor) ? 0 : 1;

        var choosenObstructedStair = lengthStairs[idLength];
        var choosenStair = (choosenWidth == WidthStair.Simple) ? choosenObstructedStair.simpleStairs[idWealth] : choosenObstructedStair.doubleStairs[idWealth];

        var choosenObstructedPattern = ChooseObstructedPattern(obstructedLocations, choosenObstructedStair, wealthLevel);

        bool isHub = false;
        
        if(!((choosenLength == LengthStair.Long && obstructedLocations.Contains(ObstructedLocation.Short)) || choosenObstructedPattern.positionStair == PositionStair.Corner))
            isHub = ChooseIfHub(wealthLevel);


        DirectionHub choosenDirectionHub = ChooseDirectionHub(isHub, wealthLevel);


        var profile = new StairProfile()
        {
            Stair = choosenStair,
            ObstructedPattern = choosenObstructedPattern,
            LengthStair = choosenLength,
            WidthStair = choosenWidth,
            Direction = choosenDirectionHub,
        };
        
        
        LoadStair(profile);
        
        
        obstructedLocations = obstructedLocations.Union(choosenObstructedPattern.obstructedLocations).ToList();
        
        return obstructedLocations;
    }

    public WidthStair ChooseWidthStair(float wealthLevel)
    {
        var firstWidth = new KeyValuePair<WidthStair, float>(WidthStair.Simple, ProceduralManager.instance.minThresholdValue);
        var secondWidth = new KeyValuePair<WidthStair, float>(WidthStair.Double, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstWidth, secondWidth, wealthLevel);
    }

    public LengthStair ChooseLengthStair(float wealthLevel, bool isLongIntegrated)
    {
        var lengths = new Dictionary<LengthStair, float>();

        if (isLongIntegrated)
        {
            lengths.Add(LengthStair.Long, 1f);
            lengths.Add(LengthStair.Medium, .5f);
            lengths.Add(LengthStair.Short, 0f);
            return ProceduralCalculations.GetRandomTFromPool(lengths, wealthLevel);
        }
        else
        {
            var firstLength = new KeyValuePair<LengthStair, float>(LengthStair.Medium, 1f);
            var secondLength = new KeyValuePair<LengthStair, float>(LengthStair.Short, 0f);

            return ProceduralCalculations.GetRandomFrom2Value(firstLength, secondLength, wealthLevel);
        }
        

    }

    public WealthStair ChooseWealthStair(float wealthLevel)
    {
        var firstWealthStair = new KeyValuePair<WealthStair, float>(WealthStair.Poor, ProceduralManager.instance.minThresholdValue);
        var secondWealthStair = new KeyValuePair<WealthStair, float>(WealthStair.Rich, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstWealthStair, secondWealthStair, wealthLevel);
        
    }
    
    
    //public DirectionHub ChooseDirectionHub()
    public ObstructedPattern ChooseObstructedPattern(List<ObstructedLocation> obstructedLocations, ObstructedStair obstructedStair, float wealthLevel)
    {
        var obstructedPatterns = new Dictionary<ObstructedPattern, float>();

        var obstructedPatternsAvailable = (from oPAvailable in obstructedStair.ObstructedPatterns
                                            where oPAvailable.obstructedLocations.Intersect(obstructedLocations).Count() == 0
                                                select oPAvailable).ToList();


        foreach (var oPAvailable in obstructedPatternsAvailable)
        {
            obstructedPatterns.Add(oPAvailable, oPAvailable.wealthValue);
        }
        
        return ProceduralCalculations.GetRandomTFromPool(obstructedPatterns, wealthLevel);
    }

    public bool ChooseIfHub(float wealthLevel)
    {
        var firstYesorNo = new KeyValuePair<bool, float>(true, ProceduralManager.instance.minThresholdValue);
        var secondYesorNo = new KeyValuePair<bool, float>(false, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstYesorNo, secondYesorNo, wealthLevel);
    }

    public DirectionHub ChooseDirectionHub(bool isHub, float wealthLevel)
    {
        if (!isHub)
            return DirectionHub.Null;
        
        var firstDirection = new KeyValuePair<DirectionHub, float>(DirectionHub.Inside, ProceduralManager.instance.minThresholdValue);
        var secondDirection = new KeyValuePair<DirectionHub, float>(DirectionHub.Outside, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstDirection, secondDirection, wealthLevel);

    }

    public void LoadStair(StairProfile stairProfile)
    {
        GameObject hub = CreateHub(stairProfile);
        ParametersStairProfile parameters = GetParameters(stairProfile);
        

        if (stairProfile.ObstructedPattern.numberStairs == 1 && stairProfile.ObstructedPattern.positionStair == PositionStair.Corner)
        {
            DuplicateAndPut(hub, new Vector3(-parameters.XAxis + parameters.DeltaHubsLength, 0 , -parameters.ZAxis), parameters.Rotation);
            DuplicateAndPut(hub, new Vector3(parameters.XAxis - parameters.DeltaHubsLength, 0 , parameters.ZAxis ), parameters.Rotation + 180);
            
            DuplicateAndPut(hub, new Vector3(parameters.XAxis - parameters.DeltaHubsLength, 0 , -parameters.ZAxis + GetWidth(stairProfile.WidthStair)), parameters.Rotation + 180);
            DuplicateAndPut(hub, new Vector3(-parameters.XAxis + parameters.DeltaHubsLength, 0 , parameters.ZAxis - GetWidth(stairProfile.WidthStair)), parameters.Rotation);
        }
        else if(stairProfile.ObstructedPattern.numberStairs == 1 && stairProfile.ObstructedPattern.positionStair != PositionStair.Corner)
        {
            DuplicateAndPut(hub, new Vector3(-parameters.XAxis, 0 , -parameters.ZAxis), parameters.Rotation);
            DuplicateAndPut(hub, new Vector3(parameters.XAxis, 0 , parameters.ZAxis), parameters.Rotation + 180);
        }
        else
        {
            var xDeltaToPut = (stairProfile.ObstructedPattern.positionStair == PositionStair.Long)? parameters.DeltaHubsLength: 0f;
            var yDeltaToPut = (stairProfile.ObstructedPattern.positionStair == PositionStair.Long)? 0f : parameters.DeltaHubsLength;
            
            DuplicateAndPut(hub, new Vector3(-parameters.XAxis + xDeltaToPut, 0 , -parameters.ZAxis + yDeltaToPut), parameters.Rotation);
            DuplicateAndPut(hub, new Vector3(parameters.XAxis + xDeltaToPut, 0 , parameters.ZAxis + yDeltaToPut), parameters.Rotation + 180);
            
            DuplicateAndPut(hub, new Vector3(-parameters.XAxis - xDeltaToPut, 0 , -parameters.ZAxis - yDeltaToPut), parameters.Rotation);
            DuplicateAndPut(hub, new Vector3(parameters.XAxis - xDeltaToPut, 0 , parameters.ZAxis - yDeltaToPut), parameters.Rotation + 180);

            if (stairProfile.ObstructedPattern.numberStairs == 3)
            {
                DuplicateAndPut(hub, new Vector3(-parameters.XAxis, 0 , -parameters.ZAxis), parameters.Rotation);
                DuplicateAndPut(hub, new Vector3(parameters.XAxis, 0 , parameters.ZAxis), parameters.Rotation + 180);
            }
        }
        
        Destroy(hub);
    }

    public GameObject CreateHub(StairProfile stairProfile)
    {
        GameObject hub = new GameObject();
        if (stairProfile.Direction != DirectionHub.Null)
        {
            GameObject firstStair = Instantiate(stairProfile.Stair, hub.transform);
            GameObject secondStair = Instantiate(stairProfile.Stair, hub.transform);

            if (stairProfile.Direction == DirectionHub.Inside)
            {
                secondStair.transform.Rotate(0, 180, 0);
                secondStair.transform.position = new Vector3(0, 0, GetWidth(stairProfile.WidthStair));
            }
            else
            {
                firstStair.transform.Rotate(0, 180, 0);
                firstStair.transform.position = new Vector3(GetLength(stairProfile.LengthStair), 0, GetWidth(stairProfile.WidthStair));
                
                secondStair.transform.position = new Vector3(-GetLength(stairProfile.LengthStair), 0, 0);
                
            }
        }
        else
        {
            GameObject soloStair = Instantiate(stairProfile.Stair, hub.transform);
            soloStair.transform.position = new Vector3(-GetLength(stairProfile.LengthStair) / 2f, 0, 0);
        }

        return hub;
    }

    public ParametersStairProfile GetParameters(StairProfile stairProfile)
    {
        var xAxis = 0f;
        var zAxis = 0f;
        var deltaHubsLength = 0f;
        var rotation = 0;

        switch (stairProfile.ObstructedPattern.positionStair)
        {
            case PositionStair.Long:
                xAxis = 0f;
                zAxis = 63f;
                deltaHubsLength = 46.5f;
                rotation = 0;
                break;
            
            case PositionStair.Short:
                xAxis = 93f;
                zAxis = 0f;
                deltaHubsLength = 31.5f;
                rotation = 90;
                break;
            
            default: // Position.Corner
                deltaHubsLength = GetLength(stairProfile.LengthStair) / 2;
                xAxis = 93f;
                zAxis = 63f;
                break;
        }

        var parameters = new ParametersStairProfile
        {
            XAxis = xAxis,
            ZAxis = zAxis,
            DeltaHubsLength = deltaHubsLength,
            Rotation = rotation
        };

        return parameters;
    }
    public void DuplicateAndPut(GameObject hub, Vector3 position, float rotation)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject duplicatedHub = Instantiate(hub, ProceduralManager.ParentMap);
            duplicatedHub.transform.position = position + new Vector3(0,7.5f * i,0);
            duplicatedHub.transform.Rotate(0, rotation, 0);
        }
        
    }
    private int GetIdLength(LengthStair lengthStair)
    {
        switch (lengthStair)
        {
            case LengthStair.Long:
                return 0;

            case LengthStair.Medium:
                return 1;

            case LengthStair.Short:
                return 2;
        }

        return 2;
    }

    private float GetWidth(WidthStair widthStair)
    {
        return (widthStair == WidthStair.Simple) ? 4 : 8;
    }

    private float GetLength(LengthStair lengthStair)
    {
        switch (lengthStair)
        {
            case LengthStair.Long:
                return 54.4f;
            
            case LengthStair.Medium:
                return 31.2f;
            
            case LengthStair.Short:
                return 19.6f;
        }
        
        Debug.LogError("Outside the bound : GetLengthStair");
        return 0;
    }
    
    /* Values
     
     Floor - width : 7 
           - height : 7.5
           
    Rectangle : - Long +/- 93, Short +/- 63
    
    Max 3 hub 
    
    Long : max 2 glued together at the center
    
    Middle : 2 side : 2 max
        
             1 side : 3 max
             
    Short : 2 side : 3
        
            1 side : 3
    
    
     
     */
    
}
