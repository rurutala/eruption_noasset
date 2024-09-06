using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover_BGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
