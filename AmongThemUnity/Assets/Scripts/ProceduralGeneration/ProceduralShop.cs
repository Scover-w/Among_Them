using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum WealthLevelShop
{
    Poor,
    Normal,
    Rich
}



public enum SizeShop
{
    Small,
    Big,
    Null
}
public class ProceduralShop : MonoBehaviour
{

    [Header("Shops")] 
    [SerializeField] 
    private GameObject[] noShops;
    
    [SerializeField] 
    private GameObject[] poorShops;
    
    [SerializeField] 
    private GameObject[] normalShops;
    
    [SerializeField] 
    private GameObject[] richShops;


    [Header("Treshold Choice Shops")] 
    [SerializeField][Range(0f,1f)]
    private float tresholdPoorNormal = 0;
    
    [SerializeField][Range(0f,1f)]
    private float tresholdNormalRich;
    
    public void LoadShops(List<ObstructedLocation> obstructedLocations, float wealthLevel)
    {
        int longSide = 10;
        int shortSide = 7;

        WealthLevelShop choosenWealthLevelShop = (wealthLevel < tresholdPoorNormal)? WealthLevelShop.Poor : 
            ((wealthLevel < tresholdNormalRich)? WealthLevelShop.Normal : WealthLevelShop.Rich);
        
        GameObject[] choosenShops = (choosenWealthLevelShop == WealthLevelShop.Poor)? poorShops : 
            ((choosenWealthLevelShop == WealthLevelShop.Normal)? normalShops : richShops);


        GameObject shopToPut;

        // TODO : Simplify this
        
        bool isShopObstructed = obstructedLocations.Contains(ObstructedLocation.Long);
        // Long Sides
        for (int i = 0; i < 2; i++)
        {
            int j = 0;
            int add = 0;
            SizeShop choosenSizeShop = SizeShop.Null;
            GameObject apartment;
            while(j < longSide)
            {
                if (j == 5 && isShopObstructed)
                {
                    add = 1;
                    shopToPut = noShops[0];
                }
                else if ((j == 4 && isShopObstructed) || (j == longSide - 1))
                {
                    add = 1;
                    shopToPut = choosenShops[0];
                    choosenSizeShop = SizeShop.Small;
                }
                else
                {
                    if (ProceduralCalculations.GetRandomValue() < wealthLevel)
                    {
                        add = 2;
                        shopToPut = choosenShops[1];
                        choosenSizeShop = SizeShop.Big;
                    }
                    else
                    {
                        add = 1;
                        shopToPut = choosenShops[0];
                        choosenSizeShop = SizeShop.Small;
                    }
                }


                int x = -100 + j * 20;
                int z = 70;
                int yRotation = 0;

                if (i == 1)
                {
                    x *= -1;
                    z *= -1;
                    yRotation = 180;
                }

                j += add;

                shopToPut = Instantiate(shopToPut);
                shopToPut.transform.position = new Vector3(x, 0, z);
                shopToPut.transform.Rotate(0, yRotation, 0);

                apartment = LoadInsideShop(choosenWealthLevelShop, choosenSizeShop, wealthLevel);
                if (apartment != null)
                {
                    apartment.transform.position = new Vector3(x, 0, z);
                    apartment.transform.Rotate(0, yRotation, 0);
                }
            }
        }
        
        isShopObstructed = obstructedLocations.Contains(ObstructedLocation.Short);
        // Short Side
        for (int i = 0; i < 2; i++)
        {
            int j = 0;
            int add = 0;
            SizeShop choosenSizeShop = SizeShop.Null;
            GameObject apartment;
            
            
            while(j < shortSide)
            {
                if (j == 3 && isShopObstructed)
                {
                    add = 1;
                    shopToPut = noShops[0];
                }
                else if ((j == 2 && isShopObstructed) || (j == shortSide - 1))
                {
                    add = 1;
                    shopToPut = choosenShops[0];
                    choosenSizeShop = SizeShop.Small;
                }
                else
                {
                    if (ProceduralCalculations.GetRandomValue() < wealthLevel)
                    {
                        add = 2;
                        shopToPut = choosenShops[1];
                        choosenSizeShop = SizeShop.Big;
                    }
                    else
                    {
                        add = 1;
                        shopToPut = choosenShops[0];
                        choosenSizeShop = SizeShop.Small;
                    }
                }


                int z = 70 - j * 20;
                int x = 100;
                int yRotation = 90;

                if (i == 1)
                {
                    x *= -1;
                    z *= -1;
                    yRotation = 270;
                }

                j += add;

                shopToPut = Instantiate(shopToPut);
                shopToPut.transform.position = new Vector3(x, 0, z);
                shopToPut.transform.Rotate(0, yRotation, 0);
                
                apartment = LoadInsideShop(choosenWealthLevelShop, choosenSizeShop, wealthLevel);
                if (apartment != null)
                {
                    apartment.transform.position = new Vector3(x, 0, z);
                    apartment.transform.Rotate(0, yRotation, 0);
                }
                
                
            }
        }
        
        // 10 long 
        // 7 short
    }

    private GameObject LoadInsideShop(WealthLevelShop wealthLevelShop, SizeShop sizeShop, float wealthValue)
    {
        if (sizeShop == SizeShop.Null)
        {
            return null;
        }
        
        var objects = AssetDatabase.LoadAllAssetsAtPath("AAssets/Prefabs/NestedPrefabs/Shop/" + wealthLevelShop + "/" + sizeShop + "/");

        List<GameObject> apartments = new List<GameObject>();
        
        var apartmentDict = new Dictionary<GameObject, float>();

        GameObject temp;
        foreach (var obj in objects)
        {
            temp = (GameObject) obj;
            apartmentDict.Add(temp, temp.GetComponent<ProceduralEntity>().wealthValue);
        }

        return Instantiate(ProceduralCalculations.GetRandomTFromPool(apartmentDict, wealthValue));
    }
}
