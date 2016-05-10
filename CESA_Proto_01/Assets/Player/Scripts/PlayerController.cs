using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    //public
    public float moveSpeed = 0.5f;
    public float moveLimitX = 10.0f;
    public float moveLimitZ = 10.0f;
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
    private bool creatingFlg = false;

    //
    public NagareManager nagareManager;


    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        ////
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

        //流れマネージャー
        //CommandNagareManager();

        if (Input.touchCount > 0)
            print(Input.touchCount);
    }

    //
    void Move()
    {
        Vector3 movePosition = transform.position;
        //Vector3 addForce = Vector3.zero;
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

    //
    //流れマネージャーに命令
    //
    void CommandNagareManager()
    {
        //流れ生成開始（タップした始め＆＆コントローラ系の場所じゃない）
        //if (Input.GetMouseButtonDown(0) && CheckTapPoint()) 
        //if ( && CheckTapPoint()) 
        //{
        //    creatingFlg = true;
        //    nagareManager.Create();
        //    return;
        //}

        //流れ生成中（タップ地点が正当＆＆スワイプ）
        if (creatingFlg && Input.GetMouseButton(0))
        {
            nagareManager.Creating();
        }

        //流れ生成
        if (Input.GetMouseButtonUp(0))
        {
            nagareManager.Activate();
            return;
        }
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
    bool    CheckTapPoint()
    {
        if (true)
        {
            return true;
        }

        return false;
    }
}
