using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherPanel : MonoBehaviour
{
    // シングルトンインスタンス
    public static WeatherPanel Instance { get; private set; }

    // 管理するパネルのリスト
    [SerializeField] private List<GameObject> panels;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 指定されたパネルをオンにし、それ以外をオフにするメソッド
    public void ShowPanel(string panelName)
    {
        bool panelFound = false;

        foreach (GameObject panel in panels)
        {
            if (panel.name == panelName)
            {
                panel.SetActive(true);  // 指定されたパネルをオンにする
                panelFound = true;
            }
            else
            {
                panel.SetActive(false); // それ以外のパネルをオフにする
            }
        }

        if (!panelFound)
        {
            Debug.LogWarning($"Panel with name {panelName} not found.");
        }
    }

    // すべてのパネルをオフにするメソッド
    public void HideAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
