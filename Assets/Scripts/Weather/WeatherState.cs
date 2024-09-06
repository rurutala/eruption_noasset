using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeatherState
{
    //天候名
    protected string weatherName;
    public string WeatherName{ get { return weatherName; } }
    //一日の初めに呼ばれる
    public abstract void StartWeather();
    //一日の終わりに呼ばれる
    public virtual void EndWeather() 
    {
        // BGMを停止
        AudioManager.Instance.StopBGM();
    }

    //歩くたびに何か天候ごとに効果を加えたチスル場合はここに追加する
    public abstract void WalkEffect();

    //ステートごとのBGMを再生する
    public abstract void PlayStateBGM();

    /*

    StartWeatherの中で天気ごとのエフェクトを配置したり、FieldObjに変更を加えたりする

    */
}

public class SunnyWeatherState : WeatherState
{
    public SunnyWeatherState()
    {
        weatherName = "晴れ";
    }


    public override void StartWeather()
    {
        Debug.Log("晴れスタート");

        // SEを鳴らす
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_Bird);

        WalkEffect();
        WeatherPanel.Instance.ShowPanel("SunnyPanel");
    }


    public override void EndWeather()
    {
        base.EndWeather();
    }

    public override void WalkEffect()
    {
        PlayerActionManager.time_correction = 1f;
        PlayerMovement.time_correction = PlayerActionManager.time_correction;
        PlayerMovement.Hunger_correction = 1f;
        PlayerMovement.Thirst_correction = 1f;
    }

    public override void PlayStateBGM()
    {
        // BGMを鳴らす
        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_Sunny);
    }
}


public class RainyWeatherState : WeatherState
{
    public RainyWeatherState()
    {
        weatherName = "雨";
    }

    public override void StartWeather()
    {
        Debug.Log("雨スタート");

        // SEを鳴らす
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_Rain);

        WalkEffect();
        WeatherPanel.Instance.ShowPanel("RainyPanel");
    }


    public override void EndWeather()
    {
        base.EndWeather();
    }
    public override void WalkEffect()
    {
        PlayerActionManager.time_correction = 1.5f;
        PlayerMovement.time_correction = PlayerActionManager.time_correction;
        PlayerMovement.Hunger_correction = 1f;
        PlayerMovement.Thirst_correction = 0;
    }
    public override void PlayStateBGM()
    {
        // BGMを鳴らす
        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_Rainy);
    }
}


public class HumidWeatherState : WeatherState
{
    public HumidWeatherState()
    {
        weatherName = "猛暑";
    }


    public override void StartWeather()
    {
        Debug.Log("猛暑スタート");

        // SEを鳴らす
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_HotDay);

        WalkEffect();
        WeatherPanel.Instance.ShowPanel("HeatPanel");
    }


    public override void EndWeather()
    {
        base.EndWeather();
    }

    public override void WalkEffect()
    {
        PlayerActionManager.time_correction = 1f;
        PlayerMovement.time_correction = PlayerActionManager.time_correction;

        PlayerMovement.Hunger_correction = 1.5f;
        PlayerMovement.Thirst_correction = 2f;
    }
    public override void PlayStateBGM()
    {
        // BGMを鳴らす
        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_Humid);
    }
}


public class ColdWeatherState : WeatherState
{
    public ColdWeatherState()
    {
        weatherName = "極寒";
    }

    public override void StartWeather()
    {
        Debug.Log("極寒スタート");

        // SEを鳴らす
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_ColdDay);

        WalkEffect();
        WeatherPanel.Instance.ShowPanel("ColdPanel");
    }


    public override void EndWeather()
    {
        base.EndWeather();
    }

    public override void WalkEffect()
    {
        PlayerActionManager.time_correction = 2f;
        PlayerMovement.time_correction = PlayerActionManager.time_correction;
        PlayerMovement.Thirst_correction = 1.5f;
        PlayerMovement.Hunger_correction = 1.5f;
    }
    public override void PlayStateBGM()
    {
        // BGMを鳴らす
        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_Cold);
    }
}


