using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DBGText_PlayerPosition : MonoBehaviour
{
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        if(player ==null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Vector2.zero;
        position.x = player.transform.position.x;
        position.y = player.transform.position.z;

        GetComponent<Text>().text = "プレイヤー座標：" + position.ToString();
    }
}
