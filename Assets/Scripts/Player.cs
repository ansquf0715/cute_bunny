using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    public float rotateSpeed;

    public GameObject bulletFactory; //�Ѿ��� ������ ����
    public GameObject firePosition; //�ѱ�

    float hAxis;
    float vAxis;
    bool runDown;

    Vector3 moveVec;
    Animator anim;
    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        Fire();
        rigid.velocity = Vector3.zero;
    }

    void PlayerMove()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        runDown = Input.GetButton("isRun");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", runDown);

        if (!(hAxis == 0 && vAxis == 0))
        {
            transform.position += moveVec * speed * Time.deltaTime;
            if (runDown == true) //�� ��
            {
                transform.position += moveVec * runSpeed * Time.deltaTime;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVec), Time.deltaTime * rotateSpeed);
        }
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //�����̽��ٸ� �Է��ϸ�
        {
            GameObject bullet = Instantiate(bulletFactory); // �Ѿ� ���忡�� �Ѿ��� �����
            bullet.transform.position = firePosition.transform.position; // �Ѿ��� �߻��Ѵ�
        }
    }

}
