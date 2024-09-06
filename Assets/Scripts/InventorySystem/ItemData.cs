using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemID;        // アイテムのID
    public string itemName;   // アイテムの名前
    public int maxStackSize = 1; // 持てる限界の個数、デフォルトは1
    public Sprite icon;       // アイテムのアイコン
    public bool isStackable = true; // 複数個持てるかどうか
}
