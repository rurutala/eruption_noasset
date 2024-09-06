using UnityEngine;

public class Item_get_berry : MonoBehaviour
{
    // アイテムが拾われたときの処理
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // アイテムがプレイヤーによって拾われたときの処理
            // ここでアイテムの処理やポイントの追加などを行う
            Debug.Log("Item collected!");

            InventorySystem.Instance.AddItem("berry1");

            // アイテムを破壊する
            Destroy(gameObject);

        }
    }
}
