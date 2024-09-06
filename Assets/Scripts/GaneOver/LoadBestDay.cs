using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LoadBestDay : MonoBehaviour
{
    public TextMeshProUGUI bestdayText;

    // Start is called before the first frame update
    void Start()
    {
        bestdayText = GetComponent<TextMeshProUGUI>();
        bestdayText.text = $"‚ ‚È‚½‚ÌÅ‘å“ú”: {PlayerPrefs.GetInt("BestDay", 0)}“ú";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
