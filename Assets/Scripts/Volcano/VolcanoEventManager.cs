using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Linq;  // DOTweenの名前空間をインポート

public class VolcanoEventManager : MonoBehaviour
{
    /*アニメーションの流れ
        ①カメラがプレイヤーから火山に移動
        ②カメラぐらぐら→噴火
        ③火山弾の抽選
        ④落ちる箇所にカメラを指定
        ⑤その後の動き
    */

    [SerializeField] Camera mainCamera;
    [SerializeField] Animator eruptionAnimator; // 噴火のアニメーションをコントロールするAnimator

    //火山カメラ位置のオフセット
    [SerializeField] Vector2 volcanoCameraPosOffset;
    // 火山のカメラ位置
    private Vector3 VolcanoCameraPosition 
    { 
        get 
        {
            if (volcano == null)
            {
                volcano = GameObject.FindGameObjectWithTag("Volcano");

                if (volcano == null)
                {
                    return Vector2.one * 32 + volcanoCameraPosOffset;

                }
            }

            return volcano.transform.position + (Vector3)volcanoCameraPosOffset;
        }
    }
    //火山のanimator
    private Animator volcanoAnimator;
    private Animator VolcanoAnimator
    {
        get
        {
            if (volcano == null)
            {
                volcano = GameObject.FindGameObjectWithTag("Volcano");

                if (volcano == null)
                {
                    Debug.Log("Volcanoが存在しません");
                    return null;
                }
            }
            else if(volcanoAnimator == null)
            {
                volcanoAnimator =volcano.GetComponent<Animator>();

                if(volcanoAnimator == null)
                {
                    Debug.Log("VolcanoのAnimatorが存在しません");
                    return null;
                }
            }

            return volcanoAnimator;
        }
    }
    
    // Playerのカメラ位置
    private Vector3 PlayerCameraPosition 
    { 
        get 
        { 
            if(player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");

                if(player == null)
                {
                    return Vector2.one * 16;

                }
            }

            return player.transform.position;
             
        } 
    }

    [SerializeField] float cameraShakingTime = 2f; // カメラの揺れ時間
    [SerializeField] float cameraMoveDuration = 1.5f; // カメラの移動時間

    //火山弾をいくつ落とすか
    [SerializeField] int fireStoneCountMinInclusive = 2;
    [SerializeField] int fireStoneCountMaxExclusive = 5; 

    //GameObjectの参照
    [SerializeField] GameObject volcano;
    [SerializeField] GameObject player;

    //火山弾のプレハブ
    [SerializeField] GameObject fireStonePrefab;
    //火山弾の降らせる際の生成場所のオフセット
    [SerializeField] Vector2 fireStoneDropPosOffset;
    //火山弾の降らせる際の生成時のカメラのオフセット
    [SerializeField] Vector2 fireStoneDropCameraOffset;
    //生成した火山弾の親
    [SerializeField] Transform fireStoneParent;
    //火山弾の落下時間
    [SerializeField] float dropDuration;

    // 着弾時のGlass変化
    [SerializeField] GameObject fireTilePrefab;
    [SerializeField] Transform fireTileParent;
    //着弾時のBeadh変化
    [SerializeField] Color firedBeachColor;


    /// <summary>
    /// 火山の噴火イベントを実行（awaitした場合イベント終了まで待機する）
    /// </summary>
    public async UniTask StartVolcanoEvent()
    {
        StaticValues.Instance.canPlayerMove = false;
        
        // シーケンスを実行
        await RunSequence();

        StaticValues.Instance.canPlayerMove = true;

    }

    private async UniTask RunSequence()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.BGM_Volcano);

        //　カメラを火山まで移動させる
        await MoveCameraToPosition(VolcanoCameraPosition);
        
        //　画面が振動し、噴火
        await PlayEruptionAnimation();


        // 火山弾を落下させ、地面を燃やす
        await DropFireStones();

        // プレイヤーにカメラを戻す
        await MoveCameraToPosition(PlayerCameraPosition);

        //BGMを現在のステートの曲に戻す
        WeatherManager.Instance.CurrentWeatherState.PlayStateBGM();
    }

    //カメラを指定座標に移動する(zはそのまま)
    private async UniTask MoveCameraToPosition(Vector3 targetPosition)
    {
        // DOTweenを使用してカメラを移動させ、イージングを適用
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z);
        await mainCamera.transform.DOMove(targetPosition, cameraMoveDuration)
            .SetEase(Ease.InOutQuad) // イージングを適用
            .AsyncWaitForCompletion(); // 処理が完了するまで待機
    }

    //カメラを指定秒数振動させる
    private async UniTask ShakeCamera(float duration)
    {
        Vector3 originalPosition = mainCamera.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);
            mainCamera.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }

        mainCamera.transform.position = originalPosition;
    }

    //噴火の演出
    private async UniTask PlayEruptionAnimation()
    {
        //画面を揺らしつつ、噴火直前アニメーションを再生
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_ShakeVolcano);
        VolcanoAnimator.SetTrigger("PreExplode");
        await ShakeCamera(cameraShakingTime);


        //噴火アニメーションを再生
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_ExplodeVolcano);
        VolcanoAnimator.SetTrigger("Explode");
        await UniTask.Delay(1000);

    }

    //火山弾が落ちてくる演出
    private async UniTask DropFireStones()
    {
        // 落とす数を選択する
        int stoneCount = Random.Range(fireStoneCountMinInclusive, fireStoneCountMaxExclusive);

        for (int i = 0; i < stoneCount; i++)
        {
            // 火山弾を落とす
            await FallFireStone();
        }
    }

    private async UniTask FallFireStone()
    {
        // 落とす座標を選択し、その座標にカメラを合わせる
        Vector3 randomPosition = GetRandomPosition();
        await MoveCameraToPosition(randomPosition + (Vector3)fireStoneDropCameraOffset);

        //着弾位置のGameObject太刀を取得しておく
        List<GameObject> burnedObjList = GetBurnedObjList(randomPosition);


        // 火山弾を画面上部に生成し、火山弾のアニメーションを落下に変更する
        var fireStoneObj = Instantiate(fireStonePrefab, randomPosition + (Vector3)fireStoneDropPosOffset, Quaternion.identity, fireStoneParent);

        // 火山弾を指定座標まで落下させ、指定座標に来たら火山弾のアニメーションを爆発に変更する
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_FallFireStone);
        await fireStoneObj.transform.DOMove(randomPosition, dropDuration).SetEase(Ease.Linear).AsyncWaitForCompletion();

        // 着弾
        AudioManager.Instance.PlaySE(AudioManager.Instance.SE_ExplodeFireStone);

        var fan = fireStoneObj.GetComponent<Animator>();
        fan.SetTrigger("Explode");

        // 着弾座標の周囲にRayを飛ばし、地面を焦土に変え、木を消す（あとあと木にアニメーションさせるかも）
        foreach (var item in burnedObjList)
        {
            //タグがグラスなら新たにFireTileを配置する
            if (item.CompareTag("Glass"))
            {
                Instantiate(fireTilePrefab, item.transform.position, Quaternion.identity, fireTileParent);
                item.SetActive(false);
            }

            if (item.CompareTag("Beach") && item.TryGetComponent<SpriteRenderer>(out var spr))
            {
                Debug.Log("砂浜！");
                spr.color = firedBeachColor;
            }

            //木の場合の処理
            if (item.CompareTag("FieldObj"))
            {
                //けす
                item.SetActive(false);
            }
        }


        await UniTask.Delay(500);

        Destroy(fireStoneObj);

        
    }

    private List<GameObject> GetBurnedObjList(Vector3 dropPos)
    {

        List<GameObject> burnedObjList = new();

        // 7x7で取得し、3x3は高確率でリストに加え、外側和帝確率でリストに加える

        for (int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                if((x == 0 && y == 0)
                   || ((Mathf.Abs(x) <= 2) && (Mathf.Abs(x) <= 2) && Random.Range(0f, 1f) <= 0.8f)
                   || (Random.Range(0f, 1f) <= 0.3f))
                {
                    // RayCastの開始位置を現在のプレイヤーの位置から少しオフセット
                    Vector3 rayOrigin = dropPos + Vector3.forward + new Vector3(x, y, 0);

                    // 火山弾の影響を及ぼす範囲にRayを飛ばし、すべてのヒットを取得する

                    var hitObjs = Physics2D.RaycastAll(rayOrigin, Vector3.back).Select(x => x.collider.gameObject).ToList();
                    burnedObjList.AddRange(hitObjs);

                    // Debug.DrawRayを使ってRayを視覚化
                    Debug.DrawRay(rayOrigin, Vector3.back, Color.black, 0.5f);
                }

                

            }
        }

        return burnedObjList;
    }


    //定数はまとめて移動する
    const int ISLAND_SIZE_W = 64;
    const int ISLAND_SIZE_H = 64;

    private Vector3 GetRandomPosition()
    {
        int rx = Random.Range(0, ISLAND_SIZE_W);
        int ry = Random.Range(0, ISLAND_SIZE_H);

        return new Vector3(rx, ry, 0);
    }
}
