using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_Option : MonoBehaviour
{

    public Button Optionbutton;
    // Start is called before the first frame update
    void Start()
    {
        Optionbutton.onClick.AddListener(() => { AudioManager.Instance.Canvas_ON(); });

        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_Title);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager.Instance.StopBGM();
        }
    }

}
