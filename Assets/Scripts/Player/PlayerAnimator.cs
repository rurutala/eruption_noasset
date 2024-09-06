using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // 装備を配置する場所
    [SerializeField] Transform equipParent;

    // 仮で棍棒をプレハブで持たせておく
    // 棍棒などはデータベースに移す
    [SerializeField] GameObject konbouPrefab;

    public bool isSwing = false;
    public bool canSwing = true;

    private void Start()
    {
        ChangeEquipment(konbouPrefab);
    }

    //プレイヤーのアニメーションを外部から変るためのコンポーネント

    // Update is called once per frame
    void Update()
    {
        //テスト用

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwingEquip();
        }
    }


    void ChangeEquipment(GameObject equipPrefab)
    {
        //子要素を削除
        foreach (Transform child in equipParent)
        {
            Destroy(child.gameObject);
        }

        //指定の武器をセット
        Instantiate(equipPrefab, equipParent);

    }

    public void SwingEquip()
    {
        if (!canSwing)
        {
            //Debug.Log("cantSwing");
            return;
        }

        animator.SetTrigger("Attack");
        isSwing = true;
    }

    //Animationからevent設定する
    public void SwingEnd()
    {
        isSwing = false;
    }
}
