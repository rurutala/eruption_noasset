using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    // BGMとSEをリストで管理
    public List<AudioClip> bgmList = new List<AudioClip>();
    public List<AudioClip> seList = new List<AudioClip>();
    public GameObject Canvas_Slider;

    [SerializeField] AudioSource bgmSource; // BGM再生用のAudioSource
    [SerializeField] AudioSource seSource;  // SE再生用のAudioSource



    #region AudioClip

    public AudioClip BGM_Sunny;
    public AudioClip BGM_Rainy;
    public AudioClip BGM_Humid;
    public AudioClip BGM_Cold;
    public AudioClip BGM_Volcano;
    public AudioClip BGM_GameOver;
    public AudioClip BGM_Title;

    public AudioClip SubBGM_Sea;

    public AudioClip SE_Move;
    public AudioClip SE_Blow;
    public AudioClip SE_Bird;
    public AudioClip SE_Rain;
    public AudioClip SE_Thunder;
    public AudioClip SE_HotDay;
    public AudioClip SE_ColdDay;

    public AudioClip SE_ShakeVolcano;
    public AudioClip SE_ExplodeVolcano;
    public AudioClip SE_FallFireStone;
    public AudioClip SE_ExplodeFireStone;

    #endregion

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでもオブジェクトを破棄しない
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        SetAudioMixerBGM(bgmSlider.value);
        SetAudioMixerSE(seSlider.value);

        bgmSlider.onValueChanged.AddListener(SetAudioMixerBGM);
        seSlider.onValueChanged.AddListener(SetAudioMixerSE);
    }

    
    /// <summary>
    /// BGMを再生する関数 フェードアウト・インで曲を入れ替える
    /// </summary>
    /// <param name="bgmClip"></param>
    /// <param name="fadeDuration"></param>
    public void PlayBGM(AudioClip bgmClip, float fadeDuration = 0.1f)
    {
        if (bgmClip != null)
        {
            // 現在のBGMが再生中ならフェードアウト
            if (bgmSource.isPlaying)
            {
                // 音量を0にフェードアウトする
                bgmSource.DOFade(0f, fadeDuration).OnComplete(() =>
                {
                    // フェードアウトが完了したら新しいBGMをセット
                    bgmSource.clip = bgmClip;
                    bgmSource.Play();

                    // 新しいBGMをフェードイン
                    bgmSource.DOFade(1f, fadeDuration);
                });
            }
            else
            {
                // 何も再生されていない場合は、即座にBGMをセットしてフェードイン
                bgmSource.clip = bgmClip;
                bgmSource.volume = 0f; // 音量を0にセット
                bgmSource.Play();
                bgmSource.DOFade(1f, fadeDuration); // フェードイン
            }
        }
    }

    /// <summary>
    /// BGMを停止する関数　フェードアウトさせる
    /// </summary>
    /// <param name="fadeDuration"></param>
    public void StopBGM(float fadeDuration = 0.1f)
    {
        // BGMが再生中であれば
        if (bgmSource.isPlaying)
        {
            // 音量を0にフェードアウトし、完了後に停止し音量を1に戻す
            bgmSource.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                bgmSource.Stop();     // フェードアウト完了後にBGMを停止
                bgmSource.volume = 1f; // 音量を1にリセット
            });
        }
    }

    /// <summary>
    /// SEを再生する関数
    /// </summary>
    /// <param name="seClip"></param>
    public void PlaySE(AudioClip seClip)
    {
        if (seClip != null)
        {
            seSource.PlayOneShot(seClip);
        }
    }

    /// <summary>
    /// BGM用のオーディオミキサーの設定
    /// </summary>
    /// <param name="value"></param>
    private void SetAudioMixerBGM(float value)
    {
        value /= 5;
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        audioMixer.SetFloat("BGM", volume);
        Debug.Log($"BGM: {volume}");
    }

    /// <summary>
    /// SE用のオーディオミキサーの設定
    /// </summary>
    /// <param name="value"></param>
    private void SetAudioMixerSE(float value)
    {
        value /= 5;
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        audioMixer.SetFloat("SE", volume);
        Debug.Log($"SE: {volume}");
    }

    /// <summary>
    /// 音量調整用のスライダーを非表示にする
    /// </summary>
    public void Slider_off()
    {
        bgmSlider.gameObject.SetActive(false);
        seSlider.gameObject.SetActive(false);
    }

    public void Canvas_ON()
    {
        Canvas_Slider.SetActive(true);
    }

    public void Canvas_OFF()
    {
        Canvas_Slider.SetActive(false);
    }
}
