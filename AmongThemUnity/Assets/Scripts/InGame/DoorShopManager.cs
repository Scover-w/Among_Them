using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShopEntity
{
    public Vector2 Position;
    public Animator Animator;
    public GameObject ParentObjects;
    public bool IsOpen;
}

public class DoorShopManager : MonoBehaviour
{
    [SerializeField] 
    private Transform positionPlayer;
    
    private List<ShopEntity> shops = new List<ShopEntity>();
    private bool isActive = false;

    private WaitForSeconds wait = new WaitForSeconds(0.2f);
    private WaitForSeconds wait2 = new WaitForSeconds(1.1f);

    private float xPPlayer;
    private float zPPlayer;

    public void EnableDoorShop()
    {
        isActive = true;
        StartCoroutine(nameof(CheckDoors));
    }
    
    public void DisableDoorShop()
    {
        isActive = false;
    }

    public void Add(ShopEntity shopEntity)
    {
        shops.Add(shopEntity);
    }

    public void Reset()
    {
        shops = new List<ShopEntity>();
    }

    IEnumerator CheckDoors()
    {
        while (isActive)
        {
            if (positionPlayer.position.y < 2f)
            {
                xPPlayer = positionPlayer.position.x;
                zPPlayer = positionPlayer.position.z;

                for(int i = 0; i < shops.Count; i++)
                {
                    var shop = shops[i];
                    if (IsNear(shop.Position))
                    {
                        if (!shop.IsOpen)
                        {
                            Debug.Log("OpenDoor");
                            shop.ParentObjects.SetActive(true);
                            shop.Animator.SetTrigger("OpenDoor");
                            shop.IsOpen = true;
                            
                            shops[i] = shop;
                        }
                    }
                    else if(shop.IsOpen)
                    {
                        Debug.Log("CloseDoor");
                        shop.Animator.SetTrigger("CloseDoor");
                        shop.IsOpen = false;
                        shops[i] = shop;

                        StartCoroutine(nameof(HideObjects), shop);
                    }
                }
            }
            
            yield return wait;
        }
    }

    private bool IsNear(Vector2 doorPosition)
    {
        Vector2 distance = new Vector2(xPPlayer - doorPosition.x , zPPlayer - doorPosition.y);

        if (distance.magnitude < 3f)
        {
            return true;
        }

        return false;
    }

    IEnumerator HideObjects(ShopEntity shopEntity)
    {
        yield return wait2;
        if (!(Math.Abs(positionPlayer.position.x) > 100.2f || Math.Abs(positionPlayer.position.z) > 70.2f))
        {
            shopEntity.ParentObjects.SetActive(false);
        }
    }
}
