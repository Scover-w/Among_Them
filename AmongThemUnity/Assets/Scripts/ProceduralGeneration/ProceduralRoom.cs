using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        GameObject room = GetRoom(wealthLevel);


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
                    
                    // Set Door interactable
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
    
    private GameObject GetRoom(float wealthLevel)
    {
        for (int i = 3; i > 0; i--)
        {
            if (rooms[i].GetComponent<ProceduralEntity>().wealthValue < wealthLevel)
                return rooms[i];
        }

        return rooms[0];
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
        Debug.Log("Room Instantiated");
        GameObject instantiatedRoom = Instantiate(room);
        instantiatedRoom.transform.position = tr.position;
    }
}
