using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    // シングルトンのインスタンスを保持するための静的変数
    private static WeatherManager instance;

    // インスタンスにアクセスするためのプロパティ
    public static WeatherManager Instance
    {
        get
        {
            // まだインスタンスがない場合、新しいインスタンスを探すか、作成する
            if (instance == null)
            {
                instance = FindObjectOfType<WeatherManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(WeatherManager).Name);
                    instance = singletonObject.AddComponent<WeatherManager>();
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

    WeatherState currentWeatherState;
    public WeatherState CurrentWeatherState 
    { 
        get 
        { 
            //現在の天気が空ならランダムで突っ込む
            if(currentWeatherState == null)
            {
                currentWeatherState = GetRandomWeather();
            }
            return currentWeatherState; 
        } 
    }

    private void Start()
    {

    }

    // 天候をスタートする
    public void StartWeather()
    {
        //天候を抽選してセットする
        currentWeatherState = GetRandomWeather();

        currentWeatherState.StartWeather();

        // 数秒後にステートのBGMをかける
        PlayStateBGM().Forget();
    }

    private async UniTask PlayStateBGM()
    {
        await UniTask.Delay(1900);

        currentWeatherState.PlayStateBGM();
    }


    public void EndWeather()
    {
        currentWeatherState.EndWeather();
    }


    // 天候を抽選する
    WeatherState GetRandomWeather()
    {
        int r = Random.Range(0,4);

        switch (r)
        {
            case 0:
                return new SunnyWeatherState();
            case 1:
                return new RainyWeatherState();
            case 2:
                return new HumidWeatherState();
            case 3:
                return new ColdWeatherState();
            default:
                return new SunnyWeatherState();
        }
    }

}
