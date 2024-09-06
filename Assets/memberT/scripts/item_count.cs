using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class item_count : MonoBehaviour
{

    public InventorySystem inventorySystem;
    public InventorySlot inventoryslot;


    private TextMeshProUGUI count;
    public Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {


        count = this.GetComponent<TextMeshProUGUI>();
        // このオブジェクトの親のTransformを取得
        Transform parentTransform = transform.parent;

        inventoryslot = parentTransform.GetComponent<InventorySlot>();

        // 親階層をたどって「InventorySystem」という名前のオブジェクトを探す
        Transform currentTransform = transform;

        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;

            // オブジェクト名が "InventorySystem" の場合、そのスクリプトを取得
            if (currentTransform.name == "InventorySystem")
            {
                inventorySystem = currentTransform.GetComponent<InventorySystem>();

                if (inventorySystem != null)
                {
                    Debug.Log("InventorySystem script found!");
                }
                else
                {
                    Debug.Log("InventorySystem script not found on the object.");
                }
                break; // 見つかったらループを抜ける
            }
        }

        if (inventorySystem == null)
        {
            Debug.Log("No InventorySystem object found in the parent hierarchy.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inventorySystem.items[inventoryslot.slotIndex] != null)
        count.text = (inventorySystem.items[inventoryslot.slotIndex]).GetComponent<Item>().quantity.ToString();
        else
        {
            count.text = "0";
        }
    }
}
