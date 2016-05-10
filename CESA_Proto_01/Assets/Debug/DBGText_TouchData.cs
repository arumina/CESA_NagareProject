using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DBGText_TouchData : MonoBehaviour
{
    public int touchIndex = 0;
    public Touch touchData;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        touchData = Input.GetTouch(touchIndex);

        string phase="phase:";
        switch (touchData.phase)
        {
            case TouchPhase.Began: phase = "Began     "; break;
            case TouchPhase.Moved: phase = "Moved     "; break;
            case TouchPhase.Stationary: phase = "Stationary"; break;
            case TouchPhase.Ended: phase = "Ended     "; break;
            case TouchPhase.Canceled: phase = "Canceled  "; break;
        }
        Vector2 position = touchData.position;

        //Vector3 work = new Vector3(touchData.position.x, Camera.main.transform.position.y, touchData.position.y);
        //work = Camera.main.ScreenToWorldPoint(work);
        //work.y = 0;

        GetComponent<Text>().text = "タッチ(" + touchIndex.ToString() + ")：" + phase + position.ToString();
    }
}
