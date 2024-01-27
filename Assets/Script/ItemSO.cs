using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO", order = 0)]
public class ItemSO : ScriptableObject {
    public byte itemID;
    public string itemName;
    public Sprite itemImage;
    public int price;
}
