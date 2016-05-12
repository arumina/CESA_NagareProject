using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    //エディタ用
    public float moveLimitX = 10.0f;
    public float moveLimitZ = 10.0f;
    public float moveDefaultSpeed = 0.5f;
    public float moveSpeedMin = 0.01f;
    public float friction = 0.0f;
    //
    public NagareManager nagareManager;

    //private
    private enum State
    {
        NEUTRAL,
        FLOWING,
    }
    private State state = State.NEUTRAL;
    private bool creatingFlg = false;
    private float moveSpeed = 0;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 nextPoint;
    private int nowRidingIndex;
    private int nowRidingNumber;
    private float chainBonus = 1.0f;

    //デバッグ用
    int dbgCount = 0;

    //
    // Use this for initialization
    //
    void Start()
    {
    }

    //
    // Update is called once per frame
    void Update()
    {
        MoveStatePattern(); //移動処理

        CommandNagareManager(); //流れマネージャー更新
    }

    //
    //移動処理パターン分岐
    void MoveStatePattern()
    {
        switch (state)
        {
            case State.NEUTRAL: Neutoral(); Debug.Log("NEUTORAL"); break;
            case State.FLOWING: Flowing(); Debug.Log("FLOWING"); break;
            default: break;
        }
    }

    //
    //流れマネージャーに命令
    void CommandNagareManager()
    {
        //流れ生成開始（タップした始め＆＆コントローラ系の場所じゃない）
        if (Input.GetMouseButtonDown(0) && CheckTapPoint())
        {
            creatingFlg = nagareManager.StartCreating();    //成功：TRUE
            return;
        }

        //流れ生成中（タップ地点が正当＆＆スワイプ）
        if (Input.GetMouseButton(0) && creatingFlg)
        {
            if (creatingFlg)
            {
                nagareManager.Creating();
            }
        }

        //流れ生成
        if (Input.GetMouseButtonUp(0))
        {
            creatingFlg = false;
            nagareManager.Activate();
            return;
        }
    }

    //
    //ニュートラル（加速していない、摩擦で減速）
    void Neutoral()
    {
        if (moveSpeed > moveSpeedMin)
        {
            //摩擦分、減速
            moveSpeed -= friction;
            if (moveSpeed <= moveSpeedMin)
            {
                moveSpeed = 0.0f;   //停止
                chainBonus = 1.0f;  //流れチェインボーナスリセット
            }
            else
            {
                Move();
            }
        }
    }

    //
    //流れに乗っている
    void Flowing()
    {
        //現在のバグ　流れに乗ってる状態で壁にぶつけて急カーブすると　止まらなくなる
        //移動実行
        Move();

        dbgCount++;
        Debug.Log(dbgCount.ToString());


        //次の流れObjに到達したら
        Vector3 distance = nextPoint - transform.position;
        if (distance.magnitude <= moveSpeed)
        {
            //さらに次の流れObjに向かうセット
            nowRidingNumber++;

            
            if(nowRidingNumber >=32)
            {
                //次の関数に32が入ると配列外なので変更
                state = State.NEUTRAL;
                return;
            }
            if ((false==nagareManager.GetActivity(nowRidingIndex, nowRidingNumber)))
            {
                //流れの終端　OR　次の流れが非アクティブの場合
                state = State.NEUTRAL;
                return;
            }
            nextPoint = nagareManager.GetNagareObj(nowRidingIndex, nowRidingNumber);
            moveDirection = nextPoint - transform.position;
            moveDirection.Normalize();
        }
    }

    //
    //移動実行（移動先がエリア内であるか確認し、外であれば修正する）
    void Move()
    {
        //移動先がエリア外ならば、各速度成分を０にする。
        Vector3 movePosition = transform.position;
        movePosition += moveDirection * moveSpeed;
        if (movePosition.x > moveLimitX || movePosition.x < -moveLimitX)
        {
            moveDirection.x = 0;
        }
        if (movePosition.z > moveLimitZ || movePosition.z < -moveLimitZ)
        {
            moveDirection.z = 0;
        }
        //移動実行
        transform.position += moveDirection * moveSpeed;
    }

    //
    //衝突処理
    //
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Nagare" && other.gameObject.activeSelf)
        {
            Debug.Log("Hit"+other.ToString());
            state = State.FLOWING;
            nowRidingIndex = other.GetComponent<Nagare>().index;
            nowRidingNumber = other.GetComponent<Nagare>().number;
            nextPoint = nagareManager.GetNagareObj(nowRidingIndex, nowRidingNumber);
            moveDirection = nextPoint - transform.position;
            moveDirection.Normalize();
            chainBonus += 0.1f;
            moveSpeed = moveDefaultSpeed * chainBonus;

            nagareManager.OffColider(nowRidingIndex);

            dbgCount = 0;
        }
    }

    //
    //タップポイントがｺﾝﾄﾛｰﾗ系でないことを確認する。
    bool CheckTapPoint()
    {
        if (true)
        {
            return true;
        }

        return false;
    }
}
