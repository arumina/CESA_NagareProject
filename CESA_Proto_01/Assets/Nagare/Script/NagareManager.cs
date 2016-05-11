using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NagareManager : MonoBehaviour
{
    //エディタ用
    public GameObject nagarePrefab;
    public int LimitNum = 4;
    public int LimitSampling = 64;
    public float Interval_Sampling = 10.0f;

    //プライベート
    private GameObject[,] nagareArray;  //流れ格納用2次元配列
    private int nowIndex = 0;
    private int nowLength = 0;
    private bool creatingFlg = false;
    private Vector3 nowMousePosition;

    //Getメソッド
    public Vector3 GetNagareObj(int index, int number) { return nagareArray[index, number].transform.position; }
    public  bool    GetActivity(int index, int number) { return nagareArray[index, number].activeSelf; }

    //デバッグ用
    public DBGText_NagareArray dbgText0;
    public DBGText_NagareArray dbgText1;
    public DBGText_NagareArray dbgText2;
    public DBGText_NagareArray dbgText3;

    // Use this for initialization
    void Start()
    {
        //流れオブジェクト配列初期化
        nagareArray = new GameObject[LimitNum, LimitSampling];
        for (int i = 0; i < nagareArray.GetLength(0); i++)
        {
            for (int j = 0; j < nagareArray.GetLength(1); j++)
            {
                GameObject nagare = (GameObject)Instantiate(nagarePrefab, Vector3.zero, Quaternion.identity); //生成
                nagareArray[i, j] = nagare;
                nagareArray[i, j].GetComponent<Nagare>().index = i;
                nagareArray[i, j].GetComponent<Nagare>().number = j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        nowMousePosition.x = Input.mousePosition.x;
        nowMousePosition.y = Input.mousePosition.y;
        nowMousePosition.z = Camera.main.transform.position.y;

        //！！！！！デバッグ用！！！！！
        for (int i = 0; i < 32; i++)
        {
            dbgText0.state[i] = nagareArray[0, i].GetComponent<Nagare>().GetState();
            dbgText1.state[i] = nagareArray[1, i].GetComponent<Nagare>().GetState();
            dbgText2.state[i] = nagareArray[2, i].GetComponent<Nagare>().GetState();
            dbgText3.state[i] = nagareArray[3, i].GetComponent<Nagare>().GetState();
        }
    }

    //
    //流れ生成開始
    //
    public bool StartCreating()
    {
        //流れが生成可能な配列があればその配列(行)のインデックス、なければ-1。
        nowIndex = FindAvailableArray();
        if (nowIndex == -1)
        {
            return false; //流れ制限超過
        }

        //生成
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(nowMousePosition);  //流れ生成開始
        nagareArray[nowIndex, 0].GetComponent<Nagare>().Initialize(worldMousePosition, Vector3.zero, 1.0f);

        nowLength = 0;
        return creatingFlg = true;
    }

    //
    //流れ生成中
    //
    public void Creating()
    {
        //流れ長制限チェック
        if (nowLength >= LimitSampling - 1)
        {
            Activate();
            return;
        }

        //サンプリング間隔チェック
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(nowMousePosition);  //スクリーン座標⇒ワールド座標
        Vector3 distance = worldMousePosition - nagareArray[nowIndex, nowLength].transform.position; //現在地点と前の生成地点との距離
        int num = (int)(distance.magnitude / Interval_Sampling); //サンプリング数（生成数）算出
        if (num == 0)
        {
            return; //前回生成地点から生成要求地点までの間隔が近すぎる場合、生成しない。
        }

        //生成処理
        for (int i = 0; i < num; i++)
        {
            Vector3 setPoint = nagareArray[nowIndex, nowLength].transform.position + distance / num;    //生成地点算出
            Vector3 setVector = setPoint - nagareArray[nowIndex, nowLength].transform.position; //前の地点から生成地点に向かうベクトル
            nagareArray[nowIndex, nowLength + 1].GetComponent<Nagare>().Initialize(setPoint, setVector, 1.0f + nowLength * 0.025f); //初期化

            nowLength++;
            if (nowLength >= LimitSampling - 1)
            {
                Activate();
                break;
            }
        }
    }

    //
    //流れ生成完了//流れ開始
    //
    public void Activate()
    {
        if (creatingFlg)
        {
            for (int i = 0; i < LimitSampling; i++)
            {
                nagareArray[nowIndex, i].GetComponent<Nagare>().Activate();
            }
            creatingFlg = false;
        }
    }

    //
    //流れが生成可能な配列があるかチェックし、生成可能であればその配列（行）のインデックスを返す。なければ-1を返す。
    //
    int FindAvailableArray()
    {
        for (int i = 0; i < LimitNum; i++)
        {
            if(i == nowIndex)
            {
                continue;
            }

            if (nagareArray[i, 0].GetComponent<Nagare>().GetState() == Nagare.State.OFF &&
                nagareArray[i, LimitSampling - 1].GetComponent<Nagare>().GetState() == Nagare.State.OFF)
            {
                return i;
            }
        }
        return -1;
    }

    //
    //
    //

    public void OffColider(int index)
    {
        for(int i=0;i<LimitSampling;i++)
        {
            nagareArray[index, i].GetComponent<SphereCollider>().enabled = false;
        }
    }
}
