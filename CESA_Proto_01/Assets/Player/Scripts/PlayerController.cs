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
    }
    private State state = State.DEFAULT;
    private Vector3 moveDirection = Vector3.zero;


    //
    public NagareManager nagareManager;


    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //
        switch (state)
        {
            case State.DEFAULT:
                Move();
                break;
            case State.NAGARE:
                Nagareteru();
                break;
            default:
                break;
        }

        CreateNagare();
    }

    //
    void Move()
    {
        Vector3 movePosition = transform.position;
        Vector3 addForce = Vector3.zero;
        addForce.x = CrossPlatformInputManager.GetAxis("Horizontal");
        addForce.z = CrossPlatformInputManager.GetAxis("Vertical");
        if (moveDirection.sqrMagnitude > 5.0f)
        {

        }

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

        state = State.DEFAULT;
    }

    //
    void CreateNagare()
    {
        if (Input.GetMouseButtonDown(0))
        {
            nagareManager.Create();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            nagareManager.Creating();
        }

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
}
