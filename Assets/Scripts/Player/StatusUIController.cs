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

        // �X���C�_�[�̍ő�l���v���C���[�X�e�[�^�X�ɐݒ�
        hpSlider.maxValue = playerStatus.MaxHP;
        hungerSlider.maxValue = playerStatus.MaxHunger;
        thirstSlider.maxValue = playerStatus.MaxThirst;
    }

    private void Update()
    {
        // �v���C���[�X�e�[�^�X�̌��ݒl���X���C�_�[�ɔ��f
        hpSlider.value = playerStatus.CurrentHP;
        hungerSlider.value = playerStatus.CurrentHunger;
        thirstSlider.value = playerStatus.CurrentThirst;
    }
}
