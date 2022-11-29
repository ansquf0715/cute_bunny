using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Inventory inventory;

    public float speed;
    public float runSpeed;
    public float rotateSpeed;

    public GameObject bulletFactory; //�Ѿ��� ������ ����
    public GameObject firePosition; //�ѱ�

    float hAxis;
    float vAxis;
    bool runDown;

    public float power; //player ���ݷ�
    public float MaxHealth; //�ִ� ü��
    private float CurrentHealth; //���� ü��

    Vector3 moveVec;
    Animator anim;
    Rigidbody rigid;

    int seedCount; //���� ����

    //���� �ɱ� �ڵ� ������ ���� -> �κ��丮 ����� ���ֱ�
    //private int countFruit;
    public GameObject tree;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        power = 1.0f;
        MaxHealth = 20.0f;
        CurrentHealth = MaxHealth;
        seedCount = 0;

        //countFruit = 0; //���� ���� ���ٰ���
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        Fire();
        rigid.velocity = Vector3.zero;
        //checkHealth();
        //Debug.Log("Player damage : " + damage);
        makeTree();
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
            //Debug.Log("Damage : " + power);
        }
    }

    public float getPower()
    {
        return power;
    }

    public void setPower(float newPower)
    {
        power = newPower;
        //Debug.Log("new Damage : " + power);
    }

    public Vector3 getPos()
    {
        return this.transform.position;
    }
    public Vector3 GetMoveVec()
    {
        return gameObject.transform.forward;
    }

    public float getMaxHealth()
    {
        return MaxHealth;
    }

    public float getHealth()
    {
        return CurrentHealth;
    }

    public void setHealth(float newHealth)
    {
        CurrentHealth = newHealth;
        FindObjectOfType<HPBarControl>().isChange();

        //Debug.Log("Set Health");
        //Debug.Log("Player cHealth : " + CurrentHealth);
    }

    private void checkHealth()
    {
        if(CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void setSeedCount(int count = 1)
    {
        //seedCount = count;
        //seedCount += count;
        seedCount++;

        Debug.Log("SetSeedCount " + seedCount);
    }

    void fixSeedCount()
    {
        GameObject.FindObjectOfType<Inventory>().setSeedCount();
    }

    GameObject TreeToMake()
    {
        return GameObject.FindObjectOfType<Seed>().selectTree();
    }

    void makeTree()
    {
        //BoxCollider makeTreeSize = GameObject.FindWithTag("FightingZone").GetComponent<FightingZone>().getBoxCollider();
        //float range_X = makeTreeSize.bounds.size.x;
        //float range_Z = makeTreeSize.bounds.size.z;

        if (seedCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                Vector3 treePoint = new Vector3(0, 0, 0);
                //if(Physics.Raycast(ray, out hit, 10000f))
                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    treePoint = hit.point;
                }
                treePoint.y = 0;
                Instantiate(TreeToMake(), treePoint, Quaternion.identity);

                //seedCount--;
                fixSeedCount();
                //Debug.Log("Tree Point : " + treePoint);
                //Instantiate(tree, treePoint, Quaternion.identity);
            }
        }

    }

    //void makeTree()
    //{
    //    //BoxCollider makeTreeSize = GameObject.FindWithTag("FightingZone").GetComponent<FightingZone>().getBoxCollider();
    //    //float range_X = makeTreeSize.bounds.size.x;
    //    //float range_Z = makeTreeSize.bounds.size.z;

    //    if(seedCount > 0)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;

    //            Vector3 treePoint = new Vector3(0, 0, 0);
    //            //if(Physics.Raycast(ray, out hit, 10000f))
    //            if (Physics.Raycast(ray, out hit, 10000f))
    //            {
    //                treePoint = hit.point;
    //            }
    //            treePoint.y = 0;
    //            Instantiate(tree, treePoint, Quaternion.identity);

    //            //seedCount--;
    //            fixSeedCount();
    //            //Debug.Log("Tree Point : " + treePoint);
    //            //Instantiate(tree, treePoint, Quaternion.identity);
    //        }
    //    }

    //}


}
