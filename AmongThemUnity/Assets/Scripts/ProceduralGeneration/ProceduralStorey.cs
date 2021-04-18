using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum RoadType
{
    Straight,
    Cross
}

[Serializable]
public struct StoreyPlace
{
    [Header("Place")] 
    public GameObject place;
    
    [Header("Cross")] 
    public List<GameObject> crossRoad;

    [Header("Straight")] 
    public List<GameObject>  straightRoad;
}

public class ProceduralStorey : MonoBehaviour
{
    [SerializeField]
    private List<StoreyPlace> places;
    
    
    public List<ObstructedLocation> LoadStoreys(List<ObstructedLocation> obstructedLocations, float wealthLevel)
    {
        StoreyPlace choosenPlace = ChooseStoreyPlace(obstructedLocations, wealthLevel);
        RoadType choosenRoad = ChooseRoadType(wealthLevel);

        var lcoations = "";
        foreach (var location in obstructedLocations)
        {
            lcoations += location + ",";
        }
        
        GameObject pfChoosenRoad = ChooseRoad((choosenRoad == RoadType.Cross)? choosenPlace.crossRoad : choosenPlace.straightRoad, obstructedLocations, wealthLevel);
        
        obstructedLocations = obstructedLocations.Union(choosenPlace.place.GetComponent<ProceduralEntity>().obstructedLocation).ToList();
        obstructedLocations = obstructedLocations.Union(pfChoosenRoad.GetComponent<ProceduralEntity>().obstructedLocation).ToList();


        LoadStorey(choosenPlace.place, pfChoosenRoad);
        
        return obstructedLocations;
    }

    private StoreyPlace ChooseStoreyPlace(List<ObstructedLocation> obstructedLocations, float wealthLevel)
    {
        var placesAvailable = (from placeAvailable in places
            where placeAvailable.place.GetComponent<ProceduralEntity>().obstructedLocation.Intersect(obstructedLocations).Count() == 0
            select placeAvailable).ToList();
        
        var placesToPick = new Dictionary<StoreyPlace, float>();

        foreach (var placeAvailable in placesAvailable)
        {
            placesToPick.Add(placeAvailable, placeAvailable.place.GetComponent<ProceduralEntity>().wealthValue);
        }

        return ProceduralCalculations.GetRandomTFromPool(placesToPick, wealthLevel);
    }

    private RoadType ChooseRoadType(float wealthLevel)
    {
        var firstRoadType = new KeyValuePair<RoadType, float>(RoadType.Cross, ProceduralManager.instance.minThresholdValue);
        var secondRoadType = new KeyValuePair<RoadType, float>(RoadType.Straight, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstRoadType, secondRoadType, wealthLevel);
    }

    private GameObject ChooseRoad(List<GameObject> roads, List<ObstructedLocation> obstructedLocations, float wealthLevel)
    {
        var roadsAvailable = (from roadAvailable in roads
                    where roadAvailable.GetComponent<ProceduralEntity>().obstructedLocation
                        .Intersect(obstructedLocations).Count() == 0
                        select roadAvailable).ToList();
        
        var road = new Dictionary<GameObject, float>();

        foreach (var roadAvailable in roadsAvailable)
        {
            road.Add(roadAvailable, roadAvailable.GetComponent<ProceduralEntity>().wealthValue);
        }

        return ProceduralCalculations.GetRandomTFromPool(road, wealthLevel); //-----------
    }

    private void LoadStorey(GameObject pfPlace, GameObject pfRoad)
    {
        GameObject place = Instantiate(pfPlace);
        place.transform.position = new Vector3(0,22.375f,0f);
        
        GameObject road = Instantiate(pfRoad);
        road.transform.position = new Vector3(0,22.375f,0f);
    }
    
}
