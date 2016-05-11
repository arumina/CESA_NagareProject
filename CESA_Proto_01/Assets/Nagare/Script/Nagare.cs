using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Nagare : MonoBehaviour
{
    public enum State
    {
        OFF,    //使われていない
        READY,  //生成中
        ON,     //生成完了＆流
    }

    //公開メンバ
    public float LIFETIME = 5.0f;   //寿命
    public int index = 0;   //（本当はパブリックだめ）
    public int number = 0;  //（本当はパブリックだめ）
    //非公開メンバ
    private State state = State.OFF;
    private Vector3 nagareDirection;
    private float lifeTimer;

    private float nagareScalar;

    //メソッド
    public State GetState() { return state; }
    public Vector3 GetNagareVector() { return nagareDirection.normalized; }

    //定数もどき

    //
    // Use this for initialization
    //
    void Start()
    {
        gameObject.SetActive(false);
        GetComponent<SphereCollider>().enabled = false; //衝突検知オフ
    }

    //
    //生成処理（流れを引いている、まだ流れていない）
    //
    public void Initialize(Vector3 setPosition, Vector3 setDirection, float setPower)
    {
        gameObject.SetActive(true); //アクティブ化
        state = State.READY;        //状態
        GetComponent<SphereCollider>().enabled = false; //衝突検知オフ

        transform.position = setPosition;   //位置
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);    //これをいれないと例の稀によくあるY軸ワープ現象が発生する
        nagareDirection = setDirection;     //向き
        nagareScalar = setPower;            //力の大きさ
        lifeTimer = LIFETIME * setPower;     //寿命

        //見た目
        GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    //
    //初期化完了（流れを引き終わり、流れ始める）
    //
    public void Activate()
    {
        if (state == State.READY)
        {
            state = State.ON;   //状態
            GetComponent<SphereCollider>().enabled = true;  //衝突検知オン

            //見た目
            GetComponent<Renderer>().material.color = new Color(0.25f, 0.25f, 0.75f, 0.5f);
        }
    }

    //
    //終了処理（寿命が尽きた）
    //
    public void End()
    {
        gameObject.SetActive(false);    //ノンアクティブ化
        state = State.OFF;              //状態
    }

    //
    // Update（流れている間）
    //
    void Update()
    {
        if (state != State.ON)
        {
            return;
        }

        //寿命カウント
        if (LifeTimer() == false)
        {
            End();  //終了処理
            return;
        }
    }

    //
    //寿命カウント
    //
    bool LifeTimer()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            return false;
        }
        return true;
    }
}
