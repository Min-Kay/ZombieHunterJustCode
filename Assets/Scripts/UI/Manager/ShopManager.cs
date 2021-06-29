using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
   
    [Header("Shop Info")]
    public Image infoIcon;
    public Text infoText;
    [Header("Player Info")]
    public Text gold;
    public ItemInfo[] inventories;
    private static ShopManager instance = null;
    private Item currItem = null;
    private Color hideColor = new Color(1, 1, 1, 0);
    public static ShopManager Instance
    {
        get
        {
            if (instance == null)
                return null;
 
            else return instance;
        }
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        
        gold.text = GameManager.Instance.gold.ToString();
        UpdateInventories();
        infoIcon.color = hideColor;
    }
    public void ShowInfo()
    {
        infoIcon.color = Color.white;
        infoIcon.sprite = currItem.icon;
        infoText.text = currItem.info;
    }
    
    public void UpdateInventories()
    {
        for (var i = 0; i < InventoryManager.Instance.items.Capacity; ++i)
        {
            if (InventoryManager.Instance.items.Count >= i+1)
            {
                inventories[i].item = InventoryManager.Instance.items[i];
                inventories[i].Setting();
            }
            else
            {
                inventories[i].item = null;
                inventories[i].Setting();
            }
        }
    }

    public void SetItem(Item item)
    {
        currItem = item;
        ShowInfo();
    }
    
    public void SellItem(Item item)
    {
        if (InventoryManager.Instance.items.Count > 1)
        {
            GameManager.Instance.gold += item.salePrice;
            if(GameManager.Instance.player.currEquipment == item && InventoryManager.Instance.items[0] != item)
                GameManager.Instance.player.ChangeItem(0);
            else if(GameManager.Instance.player.currEquipment == item && InventoryManager.Instance.items[0] == item)
                GameManager.Instance.player.ChangeItem(1);
            InventoryManager.Instance.items.Remove(item);
            Destroy(item.gameObject);
            gold.text = GameManager.Instance.gold.ToString();
            UpdateInventories();
        }
    }

    public void BuyItem()
    {
        if (GameManager.Instance.gold - currItem.purchasePrice >= 0 && InventoryManager.Instance.items.Count < InventoryManager.Instance.items.Capacity)
        {
            GameManager.Instance.gold -= currItem.purchasePrice;
            var temp = Instantiate(currItem);
            InventoryManager.Instance.items.Add(temp);
            GameManager.Instance.player.InitItem(temp);
            gold.text = GameManager.Instance.gold.ToString();
            UpdateInventories();
            if(InventoryManager.Instance.items.Count == 1)
                GameManager.Instance.player.ChangeItem(0);
        }
    }
}
