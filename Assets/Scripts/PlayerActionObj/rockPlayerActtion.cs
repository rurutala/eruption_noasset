using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockPlayerActtion : MonoBehaviour, IPlayerActionObj
{
    [SerializeField] private Animator animator;
    [SerializeField]
    private int maxHealth = 30; // 最大耐久値（HP）
    private int currentHealth;    // 現在の耐久値（HP）

    public GameObject drop_item;

    private void Start()
    {
        // ゲーム開始時に耐久値を最大値で初期化
        currentHealth = maxHealth;
    }

    public float PlayerAction(GameObject playerObj)
    {

        // 木の耐久を減らし、アニメーションさせる処理
        Debug.Log("rock処理");



        if (currentHealth != null)
        {
            animator.SetTrigger("Damage_rock");
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
            animator.SetTrigger("Down_rock");
            Die();
        }
    }

    private void Die()
    {
        // オブジェクトの破壊などの処理
        //アイテムの取得
        Debug.Log("Object has died");
        Destroy(gameObject, 2f);
        Instantiate(drop_item, transform.position, Quaternion.identity);
    }
}