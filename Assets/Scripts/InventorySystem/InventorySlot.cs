using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    public int slotIndex; // このスロットのインデックス

    private Vector2 initialPosition; // スロットの初期位置
    private Image itemImage;
    public Sprite backgroundSprite; // アイテムなし画像

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        initialPosition = rectTransform.anchoredPosition; // 初期位置を保存

        itemImage = GetComponentInChildren<Image>();
    }

    public void SetItemImage(Sprite sprite)
    {
        if (itemImage != null)
        {
            if (sprite == null)
            {
                itemImage.sprite = backgroundSprite;
            }
            else
            {
                itemImage.sprite = sprite;
            }

        }
    }

    void Update()
    {
        if (StaticValues.Instance.canPlayerMove == false)
        {
            canvas.sortingOrder = 0;
        }
        else
        {
            canvas.sortingOrder = 16;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Start");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dropPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        // インベントリシステムから選択されたアイテムを取得
        InventorySystem inventorySystem = FindObjectOfType<InventorySystem>();
        Item selectedItem = inventorySystem.GetItem(slotIndex);

        if (selectedItem != null)
        {
            // ここでItemDataのプロパティを介してアクセス
            Debug.Log($"Selected item: {selectedItem.itemData.itemName}, Quantity: {selectedItem.quantity}");

            if (selectedItem.usableItem != null)
            {
                List<string> targetTags = selectedItem.usableItem.GetTargetTags();
                Collider2D hitCollider = Physics2D.OverlapPoint(dropPosition);

                if (hitCollider != null && targetTags.Contains(hitCollider.tag))
                {
                    Debug.Log($"Dropped on {hitCollider.tag}!");
                    bool isOutOfStock = selectedItem.UseItem(hitCollider.gameObject);

                    if (isOutOfStock)
                    {
                        inventorySystem.RemoveItem(slotIndex); // 在庫がなくなった場合、インベントリから削除
                    }

                }
                else
                {
                    Debug.Log("Hit collider is not a valid target.");

                }
            }
            else
            {
                Debug.LogWarning("Selected item has no usable item defined.");

            }
        }
        else
        {
            Debug.LogWarning("No item found in the selected slot.");
 
        }
        ReturnToOriginalPosition(); 
    }

    private void ReturnToOriginalPosition()
    {
        // slotIndexに基づいて、元の位置にオフセットをかけて戻す
        rectTransform.anchoredPosition = initialPosition;
    }

    public void OnClick()
    {
        InventorySystem inventorySystem = FindObjectOfType<InventorySystem>();
        inventorySystem.SelectItem(slotIndex);
    }
}
