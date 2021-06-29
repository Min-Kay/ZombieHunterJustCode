using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public Item item;
    public Image icon;
    public Text price;
    public bool isPurchased;

    public void Start()
    {
        Setting();
    }

    public void Setting()
    {
        if (item != null)
        {
            icon.color = Color.white;
            icon.sprite = item.icon;
            if (!isPurchased)
                price.text = item.purchasePrice.ToString();
            else
                price.text = item.salePrice.ToString();

        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            price.text = null;
        }
    }

    public void SetInfo()
    {
        if(item != null)
            ShopManager.Instance.SetItem(item);
    }

    public void Sell()
    {
        if(item != null) 
            ShopManager.Instance.SellItem(item);
    }
    
}
