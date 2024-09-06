using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    // インベントリシステムのスロット
    public List<Item> items = new List<Item>(10);
    private int selectedIndex = -1; // 選択されたスロットのインデックス
    public List<InventorySlot> slots; // インベントリスロットの参照をリストで保持

    // ItemNameをキーにした辞書
    private Dictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();

    public List<GameObject> itemList = new List<GameObject>(); // 事前にアイテムリストを用意しておく
    public GameObject item_maintain; // 親オブジェクト

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject.transform.parent); // シーンをまたいでもオブジェクトを破棄しない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // アイテムリストからアイテム名をキーにして辞書に追加
        foreach (var itemObject in itemList)
        {
            Item itemComponent = itemObject.GetComponent<Item>();
            if (itemComponent != null && !itemDictionary.ContainsKey(itemComponent.itemData.itemName))
            {
                itemDictionary.Add(itemComponent.itemData.itemName, itemObject);
            }
        }
    }

    // アイテム名でアイテムを追加する
    public void AddItem(string itemName)
    {
        // アイテム名に基づいて対応するGameObjectを取得
        if (itemDictionary.TryGetValue(itemName, out GameObject itemPrefab))
        {
            // 対応するGameObjectをインスタンス化して、指定された親オブジェクトの子にする
            GameObject newItemObj = Instantiate(itemPrefab, item_maintain.transform);
            Item newItem = newItemObj.GetComponent<Item>();

            // アイテムのスタック処理
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].itemData.itemID == newItem.itemData.itemID && items[i].itemData.isStackable)
                {
                    int spaceLeft = items[i].itemData.maxStackSize - items[i].quantity;

                    if (spaceLeft > 0)
                    {
                        int quantityToAdd = Mathf.Min(spaceLeft, newItem.quantity);
                        items[i].quantity += quantityToAdd;
                        newItem.quantity -= quantityToAdd;

                        if (newItem.quantity > 0)
                        {
                            continue;
                        }
                        else
                        {
                            Destroy(newItem.gameObject);
                            return;
                        }
                    }
                }
            }

            // アイテムがスタックできない場合、または新しいスロットに追加
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    if (newItem.quantity > newItem.itemData.maxStackSize)
                    {
                        int excessQuantity = newItem.quantity - newItem.itemData.maxStackSize;

                        GameObject overflowItem = Instantiate(newItem.gameObject);
                        overflowItem.GetComponent<Item>().quantity = excessQuantity;

                        newItem.quantity = newItem.itemData.maxStackSize;
                        items[i] = newItem;
                        UpdateSlotImage(i, newItem.itemData.icon);

                        AddItem(newItem.itemData.itemName); // 再帰的に余剰分を追加
                    }
                    else
                    {
                        items[i] = newItem;
                        UpdateSlotImage(i, newItem.itemData.icon);
                    }
                    return;
                }
            }

            Debug.LogWarning("Inventory is full! Cannot add item.");
        }
        else
        {
            Debug.LogError($"Item with name {itemName} not found in the itemDictionary.");
        }
    }

    public Item GetItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            return items[index];
        }
        return null;
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items[index] = null;
            UpdateSlotImage(index, null);
        }
    }

    public void SelectItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            selectedIndex = index;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        Debug.Log("Selected item at index: " + selectedIndex);
    }

    private void UpdateSlotImage(int slotIndex, Sprite icon)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count)
        {
            slots[slotIndex].SetItemImage(icon);
        }
    }
}
