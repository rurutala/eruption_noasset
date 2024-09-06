using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandTimeManager : MonoBehaviour
{
    // シングルトンのインスタンスを保持するための静的変数
    private static IslandTimeManager instance;

    // インスタンスにアクセスするためのプロパティ
    public static IslandTimeManager Instance
    {
        get
        {
            // まだインスタンスがない場合、新しいインスタンスを探すか、作成する
            if (instance == null)
            {
                instance = FindObjectOfType<IslandTimeManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(IslandTimeManager).Name);
                    instance = singletonObject.AddComponent<IslandTimeManager>();
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


    float limitTIme; // 今日の残り時間
    [SerializeField] float maxTIme; // 最大残り時間
    public int currentDay; // 現在日数

    //仮でUIPanelをちょっとずつ暗くする
    [SerializeField] Image shadowPanel;
    [SerializeField] Slider timeSlider;
    [SerializeField] Text dayCountText;
    [SerializeField] Text dayFadeCanvasDayCountText;
    [SerializeField] Text dayFadeCanvasWeatherText;
    [SerializeField] Animator fadeCanvasAnimator;



    // Start is called before the first frame update
    void Start()
    {
        StartFirstDay().Forget();
    }

    public void ResumeTime(float time)
    {
        limitTIme -= time;

        //UIの変更
        SetTimeUI();

        // limitTimeがなくなったら次の日になる
        if(limitTIme <= 0)
        {
            Debug.Log("次の日へ");
            ChangeDay().Forget();
            
        }
    }


    // limitTimeに応じてUIを変更
    private void SetTimeUI()
    {
        timeSlider.value = limitTIme / maxTIme;
        dayCountText.text = currentDay.ToString();
        
        //DayFadeCanvasの設定
        dayFadeCanvasDayCountText.text = $"～{currentDay}日目～";
        dayFadeCanvasWeatherText.text = WeatherManager.Instance.CurrentWeatherState.WeatherName;


        if(shadowPanel != null)
        {
            //shadowPanel.color = new Color(0, 0, 0, 1 - limitTIme / maxTIme);
        }
    }


    //初日の演出
    private async UniTask StartFirstDay()
    {
        StaticValues.Instance.canPlayerMove = false;

        currentDay = 1;
        limitTIme = maxTIme;

        //新しい天気をスタート
        WeatherManager.Instance.StartWeather();

        SetTimeUI();
        await UniTask.Delay(1000);

        fadeCanvasAnimator.SetTrigger("DayStart");

        await UniTask.Delay(1000);

        StaticValues.Instance.canPlayerMove = true;
    }

    //日の切り替え
    private async UniTask ChangeDay()
    {
        StaticValues.Instance.canPlayerMove = false;

        currentDay++;
        limitTIme = maxTIme;

        //日を終えるフェード
        fadeCanvasAnimator.SetTrigger("DayEnd");

        //今の天気を終える
        WeatherManager.Instance.EndWeather();

        await UniTask.Delay(1000);
        

        //天候や災害はここで設定する        
        //新しい天気をスタート
        WeatherManager.Instance.StartWeather();

        //UIを変更
        SetTimeUI();

        await UniTask.Delay(1000);

        //日を始めるフェード
        fadeCanvasAnimator.SetTrigger("DayStart");

        await UniTask.Delay(1000);

        StaticValues.Instance.canPlayerMove = true;
    }

}
