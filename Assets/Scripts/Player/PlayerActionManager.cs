using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] PlayerStatus playerstatus;
    

    //ここら辺の情報はPlayerMovementからもらうようにしたい
    public Vector2 faceDirection; // 向いている方向
    public float moveDistance = 1.0f; // 移動する距離（タイルのサイズ）
    public float rayOffset = 0.5f; // RayCastを飛ばす際のオフセット距離

    public static float time_correction;

    //public float rayLength = 1f;
    //public float zOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // canPlayerMove出ないならActionしない
        if (!StaticValues.Instance.canPlayerMove) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 目の前にあるオブジェクトを調べてコンポーネントがヒットすればそれを実行
            CheckPlayerActionObj();
        }
    }


    void CheckPlayerActionObj()
    {
        //Debug.Log("CheckPlayerActionObj");

        //Vector3 startPosition = transform.position + new Vector3(0, 0, zOffset);
        //Vector2 direction = transform.right;
        //RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)startPosition, direction, rayLength);


        //Debug.DrawRay((Vector2)startPosition, direction * rayLength, Color.blue, 0.5f);

        // プレイヤーの現在位置から向いている方向にRayを飛ばし、すべてのヒットを取得する
        //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + (Vector3)faceDirection + Vector3.forward , Vector3.back);


        // プレイヤーの現在位置から向いている方向にRayを飛ばし、すべてのヒットを取得する
        faceDirection = transform.right;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + (Vector3)faceDirection + Vector3.forward , Vector3.back);

        //Vector2 direction = transform.right;
        //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + (Vector3)faceDirection + Vector3.forward, direction);

        // Debug.DrawRayを使ってRayを視覚化
        Debug.DrawRay(transform.position + (Vector3)faceDirection + Vector3.forward, Vector3.back, Color.blue, 0.5f);

        //Debug.Log($"hits {hits.Length}個");

        // ヒットしたオブジェクトすべてをチェック
        foreach (RaycastHit2D hit in hits)
        {
            //木などの処理を入れる
            if (hit.collider.gameObject.TryGetComponent<IPlayerActionObj>(out var actionObj))
            {
                AudioManager.Instance.PlaySE(AudioManager.Instance.SE_Blow);
                Debug.Log("CheckPlayerActionObj Found");

                // requireTimesにはActionによって消費した時間が入る
                float requiredTimes = actionObj.PlayerAction(playerObj);
                IslandTimeManager.Instance.ResumeTime(requiredTimes * time_correction);

            }

        }
    }

    public void Consume_Hunger(float consume)
    {
        playerstatus.CurrentHunger -= consume;
    }

    public void Consume_Thirst(float consume)
    {
        playerstatus.CurrentThirst -= consume;
    }

    public void Damaged(float consume)
    {
        playerstatus.CurrentHP -= consume;
    }
}
