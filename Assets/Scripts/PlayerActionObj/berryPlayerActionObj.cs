using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class berryPlayerActionObj : MonoBehaviour, IPlayerActionObj
{
    [SerializeField] private Animator animator;
    [SerializeField]
    private int maxHealth = 20; // 最大耐久値（HP）
    private int currentHealth;    // 現在の耐久値（HP）

    public GameObject drop_item;
    public Vector3 offset = new Vector3(1, 0, 0); // 対象オブジェクトからの相対位置オフセット（2Dの場合は Vector2 を使用）


    private void Start()
    {
        // ゲーム開始時に耐久値を最大値で初期化
        currentHealth = maxHealth;
    }

    public float PlayerAction(GameObject playerObj)
    {
 
        // 木の耐久を減らし、アニメーションさせる処理
        Debug.Log("berry処理");



        if (currentHealth != null)
        {
            animator.SetTrigger("Berry");
            TakeDamage(10);
        }

        // 10の時間を消費（仮）
        return 10f;
    }

    public void TakeDamage(int damage)
    {
        // ダメージを受けたときの処理
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //animator.SetTrigger("Down");
            Die();
        }
    }

    private void Die()
    {
        // オブジェクトの破壊などの処理
        //アイテムの取得
        Debug.Log("Object has died");
        currentHealth = 10;
        Vector3 position = transform.position + offset;
        Instantiate(drop_item, position, Quaternion.identity);
    }
}

