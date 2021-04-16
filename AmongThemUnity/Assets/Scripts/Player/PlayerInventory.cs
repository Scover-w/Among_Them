﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Inventory()
    {
        return _singletonInventory;
        
    }
    private static PlayerInventory _singletonInventory;
    private Dictionary<StockableItem, int> inventory;
    [SerializeField]
    private GameObject inventoryUI;
    [SerializeField]
    private List<Text> inventorySlots;
    
    // Start is called before the first frame update
    void Start()
    {
        _singletonInventory = this;
        inventory = new Dictionary<StockableItem, int>();
        StockableItem item = new StockableItem("Torchlight");
        inventoryUI.SetActive(false);
        foreach (var slot in inventorySlots)
        {
            slot.text = "";
        }
        Add(item, 2);
        Remove(item);
        Add(item, 2);
        StockableItem coin = new StockableItem("Coin");
        Add(coin);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(true);
            foreach (var item in inventory.Keys)
            {
                Debug.Log( item.GetName() + " : " + inventory[item].ToString());
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            inventoryUI.SetActive(false);
        }
    }

    StockableItem Add(StockableItem item, int number = 1)
    {
        int tempItem;
        if (inventory.TryGetValue(item, out tempItem))
        {
            inventory[item] = tempItem + number;
            inventorySlots[inventory.Count - 1].text = item.GetName() + " : " + (tempItem + number);
            
        }
        else
        {
            inventory.Add(item,number);
            inventorySlots[inventory.Count - 1].text = item.GetName() + " : " + number;
        }
        return item;
    }

    StockableItem Remove(StockableItem item, int number = 1)
    {
        int tempItem;
        if (inventory.TryGetValue(item, out tempItem))
        {
            if (tempItem - number > 0)
            {
                inventory[item] -= number;
                inventorySlots[inventory.Count - 1].text = item.GetName() + " : " + (tempItem - number);
            }
            else
            {
                inventory.Remove(item);
                inventorySlots[inventory.Count - 1].text = "";
            }
        }
        else
        {
            Debug.Log("Cette objet n'est pas dans l'inventaire");
        }
        return item;
    }
}