using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemID;        // �A�C�e����ID
    public string itemName;   // �A�C�e���̖��O
    public int maxStackSize = 1; // ���Ă���E�̌��A�f�t�H���g��1
    public Sprite icon;       // �A�C�e���̃A�C�R��
    public bool isStackable = true; // �������Ă邩�ǂ���
}
