﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_audio : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlayBGM(AudioManager.Instance.bgmList[0]);
            AudioManager.Instance.PlaySE(AudioManager.Instance.seList[0]);
        }
    }
}
