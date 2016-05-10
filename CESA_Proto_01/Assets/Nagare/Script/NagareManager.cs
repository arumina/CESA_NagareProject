using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NagareManager : MonoBehaviour
{
    //
    public GameObject nagarePrefab;
    public List<GameObject> readyList = new List<GameObject>();
    public float margin = 5.0f;

    //

    private Vector3 preMousePosition;
    private Vector3 nowMousePosition;

    // Use this for initialization
    void Start()
    {
        readyList.Clear();

        preMousePosition.x = Input.mousePosition.x;
        preMousePosition.y = Input.mousePosition.y;
        preMousePosition.z = Camera.main.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        nowMousePosition.x = Input.mousePosition.x;
        nowMousePosition.y = Input.mousePosition.y;
        nowMousePosition.z = Camera.main.transform.position.y;
    }

    //
    public void Create()
    {

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(nowMousePosition);

        GameObject nagare = (GameObject)Instantiate(
            nagarePrefab,
            worldMousePosition,
            Quaternion.identity);

        nagare.GetComponent<Nagare>().Create(Vector3.zero, 1.0f);
        readyList.Add(nagare);
    }

    public void Creating()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(nowMousePosition);

        Vector3 distance = worldMousePosition - readyList[readyList.Count - 1].transform.position;

        if (distance.sqrMagnitude > margin * margin)
        {
            int num = (int)(distance.sqrMagnitude / (margin * margin));
            
            distance /= num;
            for (int i = 0; i < num; i++)
            {
                GameObject nagare = (GameObject)Instantiate(
                    nagarePrefab,
                    readyList[readyList.Count - 1].transform.position + (distance.normalized),
                    Quaternion.identity);

                readyList.Add(nagare);

                Vector3 direction = Vector3.zero;
                direction = readyList[readyList.Count - 1].transform.position - readyList[readyList.Count - 2].transform.position;
                readyList[readyList.Count - 2].GetComponent<Nagare>().Create(direction.normalized, 1.0f + readyList.Count * 0.015f);
                readyList[readyList.Count - 1].GetComponent<Nagare>().Create(direction.normalized, 1.0f + readyList.Count * 0.015f);
            }
        }
    }

    public void Activate()
    {
        for (int i = 0; i < readyList.Count; i++)
        {
            readyList[i].GetComponent<Nagare>().Activate();
        }
        readyList.Clear();
    }
}
