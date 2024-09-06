using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // HP
    [SerializeField] private float maxHP = 100f;
    public float currentHP;

    // 空腹度
    [SerializeField] private float maxHunger = 100f;
    private float currentHunger;

    // のどの渇き
    [SerializeField] private float maxThirst = 100f;
    private float currentThirst;

    void Start()
    {
        // 初期化: すべてのステータスを最大値に設定
        currentHP = maxHP;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    void update()
    {
        if (CurrentHP <= 0 || CurrentHunger <= 0 || CurrentThirst <= 0)
        {
            animator.SetTrigger("Down");
        }

    }

    // プロパティでHPの現在値を管理
    public float CurrentHP
    {
        get => currentHP;
        set => currentHP = Mathf.Clamp(value, 0, maxHP);
    }

    // プロパティでHPの最大値を管理
    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        }
    }

    // プロパティで空腹度の現在値を管理
    public float CurrentHunger
    {
        get => currentHunger;
        set => currentHunger = Mathf.Clamp(value, 0, maxHunger);
    }

    // プロパティで空腹度の最大値を管理
    public float MaxHunger
    {
        get => maxHunger;
        set
        {
            maxHunger = value;
            currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        }
    }

    // プロパティでのどの渇きの現在値を管理
    public float CurrentThirst
    {
        get => currentThirst;
        set => currentThirst = Mathf.Clamp(value, 0, maxThirst);
    }

    // プロパティでのどの渇きの最大値を管理
    public float MaxThirst
    {
        get => maxThirst;
        set
        {
            maxThirst = value;
            currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
        }
    }
}
