using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SizeApartment
{
    Tiny,
    Small,
    Medium,
    Big
}
public class ProceduralRoom : MonoBehaviour
{
    [Header("Outside Walls")] [SerializeField]
    private GameObject[] normalWalls;
    [SerializeField]
    private GameObject[] shortWalls;
    [SerializeField]
    private GameObject[] longWalls;
    
    [Header("Doors")] [SerializeField]
    private GameObject[] doors;
    
    [Header("Rooms")] [SerializeField]
    private GameObject[] rooms;

    public void LoadRoom(List<ObstructedLocation> obstructedLocation, float wealthLevel)
    {
        GameObject wall = GetWall(GetWalls(obstructedLocation), wealthLevel);
        
        GameObject door = GetDoor(wealthLevel);

        int indexRoom = GetRoom(wealthLevel);
        GameObject room = rooms[indexRoom];
        SizeApartment sizeRoom = GetSizeByIndex(indexRoom);


        int nbDoors = wall.transform.childCount * 4;
        int choosenDoor = Random.Range(0, nbDoors);

        GameObject instantiatedWall;
        GameObject instantiatedDoor;

        int nbInstalledDoor = 0;

        for (int i = 0; i < 4; i++)
        {
            instantiatedWall = Instantiate(wall);
            instantiatedWall.transform.position = new Vector3(0f,(i + 1) * 7.5f,0f);
            
            foreach(Transform child in instantiatedWall.transform)
            {
                instantiatedDoor = Instantiate(door);
                instantiatedDoor.transform.position = child.position;
                RotateDoorTowardCenter(instantiatedDoor.transform, child);
                if (nbInstalledDoor == choosenDoor)
                {
                    LoadRoom(child, room);
                    LoadApartment(wealthLevel, child.position, sizeRoom);
                    // TODO : Set Door interactable
                }

                nbInstalledDoor++;
                Destroy(child.gameObject);
            }
        }
    }

    private GameObject[] GetWalls(List<ObstructedLocation> obstructedLocation)
    {
        if (obstructedLocation.Contains(ObstructedLocation.Short))
        {
            return shortWalls;
        }
        
        if (obstructedLocation.Contains(ObstructedLocation.Long))
        {
            return longWalls;
        }
        
        return normalWalls;
    }


    private GameObject GetWall(GameObject[] walls, float wealthLevel)
    {
        var wallDictionary = new Dictionary<GameObject, float>();
        foreach (var wallGO in walls)
        {
            wallDictionary.Add(wallGO, wallGO.GetComponent<ProceduralEntity>().wealthValue);
        }

        return ProceduralCalculations.GetRandomTFromPool(wallDictionary, wealthLevel);
    }
    private GameObject GetDoor(float wealthLevel)
    {
        var doorDictionary = new Dictionary<GameObject, float>();
        foreach (var door in doors)
        {
            doorDictionary.Add(door, door.GetComponent<ProceduralEntity>().wealthValue);
        }
        return ProceduralCalculations.GetRandomTFromPool(doorDictionary, wealthLevel);
    }
    
    private int GetRoom(float wealthLevel)
    {
        for (int i = 3; i > 0; i--)
        {
            if (rooms[i].GetComponent<ProceduralEntity>().wealthValue < wealthLevel)
                return i;
        }

        return 0;
    }

    private SizeApartment GetSizeByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return SizeApartment.Tiny;
            case 1:
                return SizeApartment.Small;
            case 2:
                return SizeApartment.Medium;
            default:
                return SizeApartment.Big;
        }
    }

    private void RotateDoorTowardCenter(Transform door, Transform location)
    {
        if (location.position.x > 99 || location.position.x < -99)
        {
            door.transform.Rotate(0f,(location.position.x > 0)? 90f : 270f,0f);
        }
        else
        {
            door.transform.Rotate(0f, (location.position.z > 0)? 0f : 180f,0f);
        }
    }

    private void LoadRoom(Transform tr, GameObject room)
    {
        GameObject instantiatedRoom = Instantiate(room);
        instantiatedRoom.transform.position = tr.position;
    }

    private void LoadApartment(float wealthValue, Vector3 position, SizeApartment sizeApartment)
    {
        var objects = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/NestedPrefabs/Apartment/" + sizeApartment + "/");

        List<GameObject> apartments = new List<GameObject>();
        
        var apartmentDict = new Dictionary<GameObject, float>();

        GameObject temp;
        foreach (var obj in objects)
        {
            temp = (GameObject) obj;
            apartmentDict.Add(temp, temp.GetComponent<ProceduralEntity>().wealthValue);
        }

        temp = Instantiate(ProceduralCalculations.GetRandomTFromPool(apartmentDict, wealthValue));
        temp.transform.position = position;
    }
}
