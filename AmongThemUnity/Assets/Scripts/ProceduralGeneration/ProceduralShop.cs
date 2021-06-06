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


    [Header("Content Shop")] 
    [SerializeField]
    private GameObject[] poorSmall;
    [SerializeField]
    private GameObject[] poorBig;
    [SerializeField]
    private GameObject[] normalSmall;
    [SerializeField]
    private GameObject[] normalBig;
    [SerializeField]
    private GameObject[] richSmall;
    [SerializeField]
    private GameObject[] richBig;


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

        // 10 units for long 
        // 7 units for short
        
        bool isShopObstructed;
        int sideValue;
        int middleObsIdx;

        GameObject shopParent;
        
        for(int h = 0; h < 2; h++)
        {
            bool isLong = (h == 0);
            sideValue = (isLong) ? longSide : shortSide;
            isShopObstructed = (isLong)
                ? obstructedLocations.Contains(ObstructedLocation.Long)
                : obstructedLocations.Contains(ObstructedLocation.Short);
            middleObsIdx = (isLong) ? 4 : 2;
            
            for (int i = 0; i < 2; i++)
            {
                int j = 0;
                int add = 0;
                SizeShop choosenSizeShop;
                GameObject shop;

                

                while (j < sideValue)
                {
                    choosenSizeShop = SizeShop.Null;
                    if (j == middleObsIdx + 1 && isShopObstructed)
                    {
                        add = 1;
                        shopToPut = noShops[0];
                    }
                    else if ((j == middleObsIdx && isShopObstructed) || (j == sideValue - 1))
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


                    int x = (isLong)? -100 + j * 20 : 100;
                    int z = (isLong)? 70 : 70 - j * 20;
                    int yRotation = (isLong)? 0 : 90;

                    if (i == 1)
                    {
                        x *= -1;
                        z *= -1;
                        yRotation = (isLong)? 180 : 270;
                    }

                    j += add;

                    shopParent = new GameObject();
                    shopParent.transform.parent = ProceduralManager.ParentMap;
                    shopParent.name = "Shop";
                    shopToPut = Instantiate(shopToPut, shopParent.transform);
                    shopToPut.transform.position = new Vector3(x, 0, z);
                    shopToPut.transform.Rotate(0, yRotation, 0);

                    if (choosenSizeShop != SizeShop.Null)
                    {
                        shop = LoadInsideShop(choosenWealthLevelShop, choosenSizeShop, wealthLevel);
                        shop.transform.parent = shopParent.transform;
                        shop.transform.position = new Vector3(x, 0, z);
                        shop.transform.Rotate(0, yRotation, 0);
                        shop.SetActive(false);
                    }
                }
            }
        }
    }

    private GameObject LoadInsideShop(WealthLevelShop wealthLevelShop, SizeShop sizeShop, float wealthValue)
    {
        GameObject[] shops;
        if(wealthLevelShop == WealthLevelShop.Poor)
        {
            shops = (sizeShop == SizeShop.Small) ? poorSmall : poorBig;
        }
        else if (wealthLevelShop == WealthLevelShop.Normal)
        {
            shops = (sizeShop == SizeShop.Small) ? normalSmall : normalBig;
        }
        else
        {
            shops = (sizeShop == SizeShop.Small) ? richSmall : richBig;
        }

        var shopDict = new Dictionary<GameObject, float>();
        
        foreach (var obj in shops)
        {
            shopDict.Add(obj, obj.GetComponent<ProceduralEntity>().wealthValue);
        }

        return Instantiate(ProceduralCalculations.GetRandomTFromPool(shopDict, wealthValue));
    }
}
