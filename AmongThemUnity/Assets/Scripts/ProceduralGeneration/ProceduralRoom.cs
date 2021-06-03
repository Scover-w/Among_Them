using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Header("Content Room")] 
    [SerializeField]
    private GameObject[] tinyContent;
    [SerializeField]
    private GameObject[] smallContent;
    [SerializeField]
    private GameObject[] mediumContent;
    [SerializeField]
    private GameObject[] bigContent;

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
            instantiatedWall = Instantiate(wall, ProceduralManager.ParentMap);
            instantiatedWall.transform.position = new Vector3(0f,(i + 1) * 7.5f,0f);
            
            foreach(Transform child in instantiatedWall.transform)
            {
                instantiatedDoor = Instantiate(door, ProceduralManager.ParentMap);
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
        GameObject instantiatedRoom = Instantiate(room, ProceduralManager.ParentMap);
        var position = tr.position;
        instantiatedRoom.transform.position = position;
        RotateRoom(instantiatedRoom.transform, position);
    }

    private void LoadApartment(float wealthValue, Vector3 position, SizeApartment sizeApartment)
    {
        GameObject[] rooms;

        switch (sizeApartment)
        {
            
            case SizeApartment.Small:
                rooms = smallContent;
                break;
            case SizeApartment.Medium:
                rooms = mediumContent;
                break;
            case SizeApartment.Big:
                rooms = bigContent;
                break;
            default: 
                rooms = tinyContent;
                break;
        }
        
        var roomDict = new Dictionary<GameObject, float>();
        foreach (var room in rooms)
        {
            roomDict.Add(room, room.GetComponent<ProceduralEntity>().wealthValue);
        }

        GameObject temp = Instantiate(ProceduralCalculations.GetRandomTFromPool(roomDict, wealthValue), ProceduralManager.ParentMap);
        temp.transform.position = position;

        RotateRoom(temp.transform, position);

    }

    private void RotateRoom(Transform room, Vector3 position)
    {
        float yRot = 0f;

        if (position.x < -100 || position.x > 100)
        {
            yRot = (position.x > 0) ? 90f : -90f;
        }
        else
        {
            yRot = (position.z > 0) ? 0f : 180f;
        }

        room.transform.Rotate(0f, yRot, 0f);
    }
}
