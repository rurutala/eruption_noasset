using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IUsableItem))]
public class Item : MonoBehaviour
{
    public ItemData itemData; // ScriptableObjectで定義されたアイテムデータ
    public int quantity; // 現在の数量（動的に変化するデータ）
    public IUsableItem usableItem; // アイテムの動作を定義するインターフェース

    private void Awake()
    {
        usableItem = GetComponent<IUsableItem>();

        if (usableItem == null)
        {
            Debug.LogWarning($"No IUsableItem found on the game object for {itemData.itemName}");
        }

        // quantityは初期値としてitemData.maxStackSizeを基に設定する
        quantity = Mathf.Clamp(quantity, 0, itemData.maxStackSize);
    }

    public bool UseItem(GameObject target)
    {
        if (usableItem != null)
        {
            usableItem.UseItem(target);

            if (quantity > 0)
            {
                quantity--;
                Debug.Log($"{itemData.itemName} used. Remaining quantity: {quantity}");

                if (quantity <= 0)
                {
                    Debug.Log($"{itemData.itemName} is out of stock.");
                    Destroy(gameObject);
                    return true; // 在庫がなくなったことを示す
                }
            }
        }
        return false;
    }
}
