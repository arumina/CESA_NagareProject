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


    // Use this for initialization
    void Start()
    {
        nagareArray = new GameObject[LimitNum, LimitSampling];

        for(int i = 0; i < nagareArray.GetLength(0); i++)
        {
            for(int j = 0; j < nagareArray.GetLength(1); j++)
            {
                GameObject nagare = (GameObject)Instantiate(nagarePrefab, Vector3.zero, Quaternion.identity); //生成
                nagareArray[i, j] = nagare;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        nowMousePosition.x = Input.mousePosition.x;
        nowMousePosition.y = Input.mousePosition.y;
        nowMousePosition.z = Camera.main.transform.position.y;
    }

    //
    //流れ生成開始
    //
    public void Create()
    {
        //流れが生成可能な配列があればその配列(行)のインデックス、なければ-1。
        nowIndex = FindAvailableArray();
        if (nowIndex == -1)
        {
            return; //流れ制限超過
        }

        
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(nowMousePosition);  //流れ生成開始
        nagareArray[nowIndex, 0].GetComponent<Nagare>().Initialize(worldMousePosition, Vector3.zero, 1.0f);

        //GameObject nagare = (GameObject)Instantiate(nagarePrefab, worldMousePosition, Quaternion.identity); //生成
        //nagare.GetComponent<Nagare>().Initialize(Vector3.zero, 1.0f);   //初期化

        //配列にセット
        //nagareArray[nowIndex, 0] = nagare;

        nowLength = 0;
        creatingFlg = true;
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
        int num = (int)(distance.magnitude / Interval_Sampling); //サンプリング数算出
        if (num == 0)
        {
            return;
        }

        //Debug.Log("SamplingNum:" + num.ToString());

        //生成処理
        for (int i = 0; i < num; i++)
        {
            Vector3 setPoint = nagareArray[nowIndex, nowLength].transform.position + distance / num;    //生成地点算出
            //GameObject nagare = (GameObject)Instantiate(nagarePrefab, setPoint, Quaternion.identity);   //生成
            Vector3 setVector = setPoint - nagareArray[nowIndex, nowLength].transform.position; //前の地点から生成地点に向かうベクトル
            nagareArray[nowIndex, nowLength+1].GetComponent<Nagare>().Initialize(setPoint,setVector, 1.0f + nowLength * 0.01f); //初期化
            //nagare.GetComponent<Nagare>().Initialize(setVector, 1.0f + nowLength * 0.1f);   //初期化

            //Debug.Log(
            //    "Mouse" + worldMousePosition.ToString() +
            //    "Start" + nagareArray[nowIndex, nowLength].transform.position.ToString() +
            //    "SetP" + setPoint.ToString()+
            //    "SetV" + setVector.ToString()
            //    );

            //Vector3 dbg = distance / num;
            //Debug.Log(dbg.ToString());

            nowLength++;
            if (nowLength >= LimitSampling - 1)
            {
                Activate();
                break;
            }

            //Debug.Log("INDEX :" + nowIndex.ToString() + " LENGTH:" + nowLength.ToString());
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
        for (int i = 0; i < nagareArray.GetLength(0); i++)
        {
            if (nagareArray[i, 0].GetComponent<Nagare>().GetState() == Nagare.State.OFF)
            {
                return i;
            }
        }
        return -1;
    }

    //
    //
    //
}
