using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int slotCount = 5;
    public List<Item> items;
    public Image[] inventories;

    private static InventoryManager instance = null;
    public static InventoryManager Instance
    { get
        {
            if (instance == null)
                return null;

            else return instance;
        }
        
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        
        items = new List<Item>(slotCount);
    }
    
    public void InitInventory()
    {
        for (var i = 0; i < slotCount; ++i)
        {
            if (items.Count > i)
            {
                inventories[i].sprite = items[i].icon;
                inventories[i].color = Color.white;
            }
            else
            {
                inventories[i].sprite = null;
                inventories[i].color = Color.clear;
            }
        }
        
    }

}
