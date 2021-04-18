using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEverythingExcept : MonoBehaviour
{

    [SerializeField] 
    private List<GameObject> objectsToKeep;
    
    public bool destroyObj = true;
    private bool tempDestroyObj = true;

    private void Update()
    {
        if (destroyObj != tempDestroyObj)
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
            foreach (var gObject in allObjects)
            {
                if (!objectsToKeep.Contains(gObject))
                {
                    Destroy(gObject);
                    
                }
                    
            }
            ProceduralManager.instance.Shuffle();
            tempDestroyObj = destroyObj;
        }
    }
}
