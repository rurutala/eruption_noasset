using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{
    public PlayerStatus playerStatus;

    public Slider hpSlider;
    public Slider hungerSlider;
    public Slider thirstSlider;

    private void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();

        // スライダーの最大値をプレイヤーステータスに設定
        hpSlider.maxValue = playerStatus.MaxHP;
        hungerSlider.maxValue = playerStatus.MaxHunger;
        thirstSlider.maxValue = playerStatus.MaxThirst;
    }

    private void Update()
    {
        // プレイヤーステータスの現在値をスライダーに反映
        hpSlider.value = playerStatus.CurrentHP;
        hungerSlider.value = playerStatus.CurrentHunger;
        thirstSlider.value = playerStatus.CurrentThirst;
    }
}
