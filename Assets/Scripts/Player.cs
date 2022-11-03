using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    public float rotateSpeed;

    public GameObject bulletFactory; //총알을 생산할 공장
    public GameObject firePosition; //총구

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
            if (runDown == true) //뛸 때
            {
                transform.position += moveVec * runSpeed * Time.deltaTime;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVec), Time.deltaTime * rotateSpeed);
        }
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //스페이스바를 입력하면
        {
            GameObject bullet = Instantiate(bulletFactory); // 총알 공장에서 총알을 만들고
            bullet.transform.position = firePosition.transform.position; // 총알을 발사한다
        }
    }

}
