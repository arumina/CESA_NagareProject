using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Nagare : MonoBehaviour
{
    public enum State
    {
        OFF,
        READY,
        ON,
    }

    //公開メンバ
    public float lifeTime = 5.0f;
    //非公開メンバ
    private State state = State.OFF;
    private Vector3 nagareVector;
    private Vector3 nagareDirection;
    private float nagareScalar;

    //メソッド
    public Vector3 GetNagareVector() { return nagareVector = nagareDirection.normalized; }
    public State GetState() { return state; }

    //
    private float BUFFER_LIFETIME;

    // Use this for initialization
    void Start()
    {
        BUFFER_LIFETIME = lifeTime;

        state = State.OFF;
        gameObject.SetActive(false);

    }

    //生成処理（流れを引いている、まだ流れていない）
    public void Initialize(Vector3 setPosition, Vector3 setDirection, float setPower)
    {
        gameObject.SetActive(true);
        state = State.READY;

        transform.position = setPosition;
        nagareDirection = setDirection; //向き
        nagareScalar = setPower;        //大きさ
        lifeTime = BUFFER_LIFETIME * setPower;           //寿命（強さ２倍なら寿命二倍）
        //lifeTime *= 1 - setPower;   //強さ２倍なら寿命半分

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        GetComponent<SphereCollider>().enabled = false;

    }

    //初期化完了（流れを引き終わり、流れ始める）
    public void Activate()
    {
        if (state == State.READY)
        {
            state = State.ON;
            GetComponent<Renderer>().material.color = new Color(0.25f, 0.25f, 0.75f, 0.5f);
            GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            state = State.OFF;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.ON)
        {
            return;
        }

        //寿命
        if (LifeTimer() == false)
        {
            //自殺処理とか
            state = State.OFF;
            gameObject.SetActive(false);
            return;
        }

        //流れ弱くなるてきな？
        nagareScalar *= 0.999f;

    }

    //寿命カウント
    bool LifeTimer()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            return false;
        }
        return true;
    }
}
