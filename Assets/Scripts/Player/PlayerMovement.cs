using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 1.0f; // 移動する距離（タイルのサイズ）
    public float initialDelay = 0.5f; // 最初の移動が発生するまでの遅延時間
    public float repeatRate = 0.2f; // キーを押し続けたときの移動の繰り返し間隔
    public float rayOffset = 0.5f; // RayCastを飛ばす際のオフセット距離
    public List<string> walkableTags; // 歩行可能な地形を示すタグのリスト
    public List<string> unwalkableTags; // 歩行不可な地形を示すタグのリスト

    private Vector2 moveDirection;
    private Collider2D playerCollider;
    private Coroutine movementCoroutine;

    private Vector2 faceDirection; // 顔の向き
    private PlayerAnimator playerAnimator;
    private PlayerActionManager playerActionManager;

    [SerializeField] private float onewalk_Hunger;
    [SerializeField] private float onewalk_Thirst;

    public static float Hunger_correction = 1f;
    public static float Thirst_correction = 1f;
    public static float time_correction = 1f;

    public float consume_walktime;

    [SerializeField] float eruption_rand;

    //火山イベント（仮でここに置く）
    [SerializeField] VolcanoEventManager volcanoEvent;

    // 火床のダメージ
    [SerializeField] float fireGroundDamage = 2f;

    void Start()
    {
        // 自身のコライダーを取得
        playerCollider = GetComponent<Collider2D>();

        playerAnimator = GetComponent<PlayerAnimator>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    void Update()
    {
        HandleInput();
    }

    // 入力に基づいて移動方向を設定する関数
    void HandleInput()
    {
        // canPlayerMove出ないなら移動しない
        if (!StaticValues.Instance.canPlayerMove) return;

        //Swingアニメーション中なら移動しない
        if (playerAnimator != null && playerAnimator.isSwing) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartMovement(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StartMovement(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            StartMovement(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StartMovement(Vector2.right);
        }

        // キーを離したときにコルーチンを停止
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            StopMovement();
        }
    }

    // 移動を開始する関数
    void StartMovement(Vector2 direction)
    {
        //移動中はスイングできない
        //Debug.Log("移動中はスイングできない");
        playerAnimator.canSwing = false;

        moveDirection = direction;

        //入力の方向に顔の向きを変更する
        faceDirection = direction;
        ChangeFaceDirection();
        
        Move(); // 最初の移動
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(RepeatMove());
    }

    // 移動を停止する関数
    void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }

        //停止中はスイングできる
        //Debug.Log("停止中はスイングできる");
        playerAnimator.canSwing = true;
    }

    // 移動処理を行う関数
    void Move()
    {
        // canPlaerMoveでないならRepeat移動を停止する
        if (!StaticValues.Instance.canPlayerMove && movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }

        // canPlayerMove出ないなら移動しない
        if (!StaticValues.Instance.canPlayerMove) return;
        //Debug.Log("Move");

        // RayCastの開始位置を現在のプレイヤーの位置から少しオフセット
        Vector3 rayOrigin = transform.position + (Vector3)moveDirection + Vector3.forward;

        // プレイヤーの現在位置から移動方向にRayを飛ばし、すべてのヒットを取得する
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector3.back);

        // Debug.DrawRayを使ってRayを視覚化
        Debug.DrawRay(rayOrigin, Vector3.back, Color.red, 0.5f);

        //Debug.Log($"rayOrigin {rayOrigin} , hitCount {hits.Length}");

        bool isHitWalkable = false;
        bool isHitFireGround = false;

        // ヒットしたオブジェクトすべてをチェック
        foreach (RaycastHit2D hit in hits)
        {
            ////木などの処理を入れる
            //if(hit.collider.CompareTag("tree"))
            //{
            //    Debug.Log("hit");
            //}

            //FireGroundを踏んだ時
            if (hit.collider.CompareTag("FireGround"))
            {
                isHitFireGround = true;
            }

            //unwalkableTagsがヒットしたら移動しない
            if (IsUnWalkableTag(hit.collider.tag))
            {
                return;
            }

            // 当たったオブジェクトのタグがwalkableTagsのいずれかに一致する場合
            if (IsWalkableTag(hit.collider.tag))
            {
                isHitWalkable = true;
            }
        }

        // 移動
        if (isHitWalkable)
        {
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AudioManager.Instance.SE_Move);

            // 位置を更新
            transform.position += (Vector3)moveDirection * moveDistance;

            // 床がFireGroundならダメージを受ける
            if (isHitFireGround)
            {
                playerActionManager.Damaged(fireGroundDamage);
            }

            IslandTimeManager.Instance.ResumeTime(consume_walktime * time_correction);
            playerActionManager.Consume_Hunger(onewalk_Hunger * Hunger_correction);
            playerActionManager.Consume_Thirst(onewalk_Thirst * Thirst_correction);

            if(Random.Range(0,1f) <= eruption_rand * IslandTimeManager.Instance.currentDay)
            {
                volcanoEvent.StartVolcanoEvent().Forget();
            }
        }
    }

    //顔の向きの変更を行う関数
    public void ChangeFaceDirection()
    {
        // 向きの変更
        if (faceDirection != Vector2.zero)
        {
            // 向きを変更するために回転角度を計算
            float angle = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    // 当たったタグが歩行可能タグに含まれているかをチェックする関数
    bool IsWalkableTag(string tag)
    {
        return walkableTags.Contains(tag);
    }

    bool IsUnWalkableTag(string tag)
    {
        return unwalkableTags.Contains(tag);
    }

    // 移動を繰り返し処理するコルーチン
    IEnumerator RepeatMove()
    {
        yield return new WaitForSeconds(initialDelay); // 最初の遅延
        while (true)
        {
            Move();
            yield return new WaitForSeconds(repeatRate);
        }
    }
}
