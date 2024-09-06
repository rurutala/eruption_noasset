using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverManager : MonoBehaviour
{
    public static bool gameover;

    public string gameoverScenename;

    private PlayerStatus playerStatus;
    public PlayerStatus PlayerStatus { 
        get 
        {
            if (playerStatus == null)
            {
                playerStatus = FindObjectOfType<PlayerStatus>();

                if(playerStatus == null )
                {
                    Debug.Log("シーン上にPlayerStatusがありません");
                    return null;
                }
            }
            return playerStatus;
        } 
    }

    public float delayBeforeLoading = 1.5f; // シーン移動までの遅延時間（秒）

    // Start is called before the first frame update
    void Start()
    {
        gameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameover) return;
        if(PlayerStatus.CurrentHP <= 0 || PlayerStatus.CurrentHunger <= 0 || PlayerStatus.CurrentThirst <= 0)
        {
            //プレイヤーが倒れる

            // 操作不能にする
            StaticValues.Instance.canPlayerMove = false;

            gameover = true;
            if (IslandTimeManager.Instance.currentDay > PlayerPrefs.GetInt("BestDay", 0))
            {
                PlayerPrefs.SetInt("BestDay", IslandTimeManager.Instance.currentDay);
            }
            StartCoroutine(LoadSceneAfterDelay());
        }
    }
    private IEnumerator LoadSceneAfterDelay()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_Blow);

        yield return new WaitForSeconds(delayBeforeLoading);
        // シーンのロード
        SceneManager.LoadScene(gameoverScenename);
    }

}
