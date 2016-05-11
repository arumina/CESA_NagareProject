using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DBGText_NagareArray : MonoBehaviour
{
    public int index = 0;
    public Nagare.State[] state = new Nagare.State[32];

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string str = "INDEX：" + index.ToString() + " ";

        for (int i = 0; i < 32; i++)
        {
            switch (state[i])
            {
                case Nagare.State.OFF: str += "○"; break;
                case Nagare.State.ON: str += "×"; break;
                case Nagare.State.READY: str += "△"; break;
            }
        }

        GetComponent<Text>().text = str;
    }
}
