using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEverythingExcept : MonoBehaviour
{

    [SerializeField] 
    private List<GameObject> objectsToKeep;
    
    public bool destroyObj = true;
    private bool tempDestroyObj = true;

    [Range(0f, 1f)] [Header("Is used when Test function activated")] 
    public float wealthValue = 0f;

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
            //Test();
            tempDestroyObj = destroyObj;
        }
    }

    public void Test()
    {
        /*Debug.Log(ProceduralCalculations.GetRandomFrom2Value(new KeyValuePair<WidthStair, float>(WidthStair.Simple, .3f),
            new KeyValuePair<WidthStair, float>(WidthStair.Double, .7f), wealthValue));*/
    }
}
