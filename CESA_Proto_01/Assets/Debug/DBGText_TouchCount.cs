using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DBGText_TouchCount : MonoBehaviour
{
    public int touchCount = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        touchCount = Input.touchCount;
        GetComponent<Text>().text = "タッチ数：" + touchCount.ToString();
    }
}
