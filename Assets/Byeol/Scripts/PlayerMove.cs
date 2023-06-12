using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    Forward,
    Back,
    Left,
    Right
}

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float runSpeed = 10f;
    public float rotateSpeed = 10f;

    Animator anim;
    float hAxis;
    float vAxis;
    bool runDown;
    Vector3 moveVec;

    public KeyCode hAxisPositiveKey = KeyCode.D;
    public KeyCode hAxisNegativeKey = KeyCode.A;
    public KeyCode vAxisPositiveKey = KeyCode.W;
    public KeyCode vAxisNegativeKey = KeyCode.S;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MakeMove();
    }

    void MakeMove()
    {

        hAxis = GetAxisInput(hAxisPositiveKey, hAxisNegativeKey);
        vAxis = GetAxisInput(vAxisPositiveKey, vAxisNegativeKey);
        runDown = Input.GetButton("isRun");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", runDown);

        ICommand moveCommand = null;

        if (!(hAxis == 0 && vAxis == 0))
        {
            if (runDown == true)
            {
                moveCommand = new PlayerRunCommand(transform, moveVec, runSpeed, rotateSpeed);
            }
            else
            {
                moveCommand = new PlayerMoveCommand(transform, moveVec, speed, rotateSpeed);
            }
        }

        if (moveCommand != null)
        {
            moveCommand.Execute();
        }
    }

    float GetAxisInput(KeyCode positiveKey, KeyCode negativeKey)
    {
        float input = 0f;
        if (Input.GetKey(positiveKey))
            input += 1f;
        if (Input.GetKey(negativeKey))
            input -= 1f;
        return input;
    }

    public void SetDirectionKey(KeyCode key, MoveDirection direction)
    {
        switch(direction)
        {
            case MoveDirection.Forward:
                vAxisPositiveKey = key;
                break;
            case MoveDirection.Back:
                vAxisNegativeKey = key;
                break;
            case MoveDirection.Left:
                hAxisNegativeKey = key;
                break;
            case MoveDirection.Right:
                hAxisPositiveKey = key;
                break;
            default:
                break;
        }
    }
}

public interface ICommand
{
    void Execute();
}

public class PlayerMoveCommand : ICommand
{
    private Transform transform;
    private Vector3 moveVec;
    private float speed;
    private float rotateSpeed;

    public PlayerMoveCommand(Transform transform, Vector3 moveVec, float speed, float rotateSpeed)
    {
        this.transform = transform;
        this.moveVec = moveVec;
        this.speed = speed;
        this.rotateSpeed = rotateSpeed;
    }

    public void Execute()
    {
        transform.position += moveVec * speed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVec), Time.deltaTime * rotateSpeed);
    }
}

public class PlayerRunCommand : ICommand
{
    private Transform transform;
    private Vector3 moveVec;
    private float runSpeed;
    private float rotateSpeed;

    public PlayerRunCommand(Transform transform, Vector3 moveVec, float runSpeed, float rotateSpeed)
    {
        this.transform = transform;
        this.moveVec = moveVec;
        this.runSpeed = runSpeed;
        this.rotateSpeed = rotateSpeed;
    }

    public void Execute()
    {
        transform.position += moveVec * runSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVec), Time.deltaTime * rotateSpeed);
    }
}