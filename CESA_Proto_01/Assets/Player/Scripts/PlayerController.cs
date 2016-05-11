using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    //エディタ用
    public float moveSpeed = 0.5f;
    public float moveLimitX = 10.0f;
    public float moveLimitZ = 10.0f;
    public float createTimeLimit = 3.0f;
    //
    public NagareManager nagareManager;

    //private
    private enum State
    {
        DEFAULT = 0,
        NAGARE,
        NEUTRAL,

    }
    private State state = State.DEFAULT;
    private Vector3 moveDirection = Vector3.zero;
    private float time = 0;
    private float createTimer = 0;
    private bool creatingFlg = false;

    //
    // Use this for initialization
    //
    void Start()
    {
    }

    //
    // Update is called once per frame
    //
    void Update()
    {
        //switch (state)
        //{
        //    case State.DEFAULT:
        //        Move();
        //        break;
        //    case State.NAGARE:
        //        Nagareteru();
        //        break;
        //    case State.NEUTRAL:
        //        Neutoral();
        //        break;
        //    default:
        //        break;
        //}

        CommandNagareManager(); //流れマネージャー更新
    }

    //
    //流れマネージャーに命令
    //
    void CommandNagareManager()
    {
        //流れ生成開始（タップした始め＆＆コントローラ系の場所じゃない）
        if (Input.GetMouseButtonDown(0) && CheckTapPoint())
        {
            creatingFlg = nagareManager.StartCreating();    //成功：TRUE
            createTimer = 0;
            return;
        }

        //流れ生成中（タップ地点が正当＆＆スワイプ）
        if (Input.GetMouseButton(0) && creatingFlg) 
        {
            if (creatingFlg)
            {
                nagareManager.Creating();

                //流れ生成時間制限カウント
                //createTimer += Time.deltaTime;
                //if (createTimer > createTimeLimit)
                //{
                //Debug.Log("!!! Create TimeOver  !!!");
                //}
            }
        }

        //流れ生成
        if (Input.GetMouseButtonUp(0))
        {
            creatingFlg = false;
            nagareManager.Activate();
            createTimer = 0;
            return;
        }
    }

    //
    void Move()
    {
        Vector3 movePosition = transform.position;
        moveDirection.x = CrossPlatformInputManager.GetAxis("Horizontal");
        moveDirection.z = CrossPlatformInputManager.GetAxis("Vertical");
        movePosition += moveDirection * moveSpeed;
        if (movePosition.x > moveLimitX || movePosition.x < -moveLimitX)
        {
            moveDirection.x = 0;
        }
        if (movePosition.z > moveLimitZ || movePosition.z < -moveLimitZ)
        {
            moveDirection.z = 0;
        }
        transform.position += moveDirection * moveSpeed;
    }

    void Nagareteru()
    {
        Vector3 movePosition = transform.position;
        movePosition += moveDirection * moveSpeed * 1.25f;
        if (movePosition.x > moveLimitX || movePosition.x < -moveLimitX)
        {
            moveDirection.x = 0;
        }
        if (movePosition.z > moveLimitZ || movePosition.z < -moveLimitZ)
        {
            moveDirection.z = 0;
        }
        transform.position += moveDirection * moveSpeed * 1.25f;

        state = State.NEUTRAL;
        time = 1.25f;
    }

    void Neutoral()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            state = State.DEFAULT;
            return;
        }

        Vector3 movePosition = transform.position;
        movePosition += moveDirection * moveSpeed * time;
        if (movePosition.x > moveLimitX || movePosition.x < -moveLimitX)
        {
            moveDirection.x = 0;
        }
        if (movePosition.z > moveLimitZ || movePosition.z < -moveLimitZ)
        {
            moveDirection.z = 0;
        }
        transform.position += moveDirection * moveSpeed * time;

        state = State.NEUTRAL;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Nagare")
        {
            state = State.NAGARE;
            moveDirection = other.GetComponent<Nagare>().GetNagareVector();
        }
    }


    //
    //タップポイントがｺﾝﾄﾛｰﾗ系でないことを確認する。
    //
    bool CheckTapPoint()
    {
        if (true)
        {
            return true;
        }

        return false;
    }
}
