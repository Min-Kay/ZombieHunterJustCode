using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum Kind
    {
        Etc,
        Weapon,
        Important,
        Consumable
    }

    public Kind kind = Kind.Etc;
    public Sprite icon;
    [TextArea] public string info;
    public int salePrice = 0;
    public int purchasePrice = 0;
}
