using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValues : MonoBehaviour
{
    // シングルトンのインスタンスを保持するための静的変数
    private static StaticValues instance;

    // インスタンスにアクセスするためのプロパティ
    public static StaticValues Instance
    {
        get
        {
            // まだインスタンスがない場合、新しいインスタンスを探すか、作成する
            if (instance == null)
            {
                instance = FindObjectOfType<StaticValues>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(StaticValues).Name);
                    instance = singletonObject.AddComponent<StaticValues>();
                }

                // このインスタンスは破棄されないようにする
                //DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    // シングルトンがすでに存在する場合、追加のインスタンスを防止する
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
    }

    /// <summary>
    /// プレイヤーの行動の可否を制御、イベント中や、日付の変更、プレイヤー死亡時等にfalseにする
    /// </summary>
    public bool canPlayerMove = true;
}
