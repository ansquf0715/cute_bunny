using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    Inventory inventory;

    public float speed;
    public float runSpeed;
    public float rotateSpeed;

    public GameObject bulletFactory; //총알을 생산할 공장
    public GameObject firePosition; //총구

    float hAxis;
    float vAxis;
    bool runDown;
    //bool isAttacking;

    public float power; //player 공격력
    public float MaxHealth; //최대 체력
    private float CurrentHealth; //현재 체력

    Vector3 moveVec;
    Animator anim;
    Rigidbody rigid;

    int seedCount; //씨앗 개수
    bool CanUseSeed = false; //인벤토리에서 사용되면

    public GameObject[] trees;

    BoxCollider fightingZoneCollider;
    bool isInsideZone = false;

    bool playerDied = false;

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        anim = GetComponentInChildren<Animator>();
        fightingZoneCollider = GameObject.FindWithTag("FightingZonePlane").GetComponent<BoxCollider>();

        anim.SetBool("isAttacking", false);
        anim.SetBool("isDie", false);

        rigid = GetComponent<Rigidbody>();
        power = 2.0f;
        MaxHealth = 20.0f;
        CurrentHealth = MaxHealth;
        //seedCount = inventory.getCountSeed();

        //countFruit = 0; //씨앗 개수 없앨거임
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDied == false)
        {
            PlayerMove();
            Fire();
            rigid.velocity = Vector3.zero;
            //checkHealth();
            //Debug.Log("Player damage : " + damage);

            if (inventory.getCountSeed() > -1)
            {
                makeTree();
            }
            if (CurrentHealth <= 0)
            {
                checkPlayerHP();

                //this.gameObject.transform.position = new Vector3(0, 0, 0);
            }
        }

    }

    public bool checkDie()
    {
        return playerDied;
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
            anim.SetBool("isAttacking", true);
            StartCoroutine(CarrotDelay());
            GameObject bullet = Instantiate(bulletFactory); // 총알 공장에서 총알을 만들고
            bullet.transform.position = firePosition.transform.position; // 총알을 발사한다
            //Debug.Log("Damage : " + power);
            StartCoroutine(AttackingDelay());
        }
    }

    IEnumerator AttackingDelay()
    {
        float delayTime = 0.5f;
        yield return new WaitForSeconds(delayTime);
        anim.SetBool("isAttacking", false);
    }

    IEnumerator CarrotDelay()
    {
        yield return new WaitForSeconds(1f);
    }

    public float getPower()
    {
        return power;
    }

    public void setPower(float newPower)
    {
        power += newPower;
        Debug.Log("new Damage : " + power);
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

    //public void setHealth(float newHealth)
    //{
    //    CurrentHealth = newHealth;
    //    FindObjectOfType<HPBarControl>().isChange();

    //    //Debug.Log("Set Health");
    //    //Debug.Log("Player cHealth : " + CurrentHealth);
    //}




    public void setHealth(float newHealth) //temp 사용 -> 고쳐야될듯
    {
        CurrentHealth += newHealth;
        FindObjectOfType<HPBarControl>().isChange();
        //Debug.Log("현재 체력 : " + CurrentHealth);

        //Debug.Log("Set Health");
        //Debug.Log("Player cHealth : " + CurrentHealth);
    }



    public void checkPlayerHP() //체력을 확인하고 0되면 애니메이션 나오게하기
    {
        //Debug.Log("check Player HP 불러짐");
        anim.SetBool("isDie", true);
        playerDied = true;
        StartCoroutine(Die());
        //anim.SetBool("isDie", false);
        StartCoroutine(rebirth()); //5초가 지나고
        setHealth(20);
        StartCoroutine(DelayMove());
        //setHealth(20);
    }

    IEnumerator DelayMove()
    {
        yield return new WaitForSeconds(5f);
        this.gameObject.transform.position = new Vector3(0, 0, 0);

    }

    IEnumerator Die()
    {
        //Debug.Log("Die animation");
        yield return new WaitForSeconds(5f);
    }

    IEnumerator rebirth()
    {
        //Debug.Log("Rebirth");
        yield return new WaitForSeconds(5.0f);
        anim.SetBool("isDie", false);
        playerDied = false;

    }

    //IEnumerator AttackingDelay()
    //{
    //    float delayTime = 0.5f;
    //    yield return new WaitForSeconds(delayTime);
    //    anim.SetBool("isAttacking", false);
    //}

    private void checkHealth()
    {
        if(CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int setSeedCount()
    {
        //Debug.Log("남은 씨앗 수 " + inventory.getCountSeed());
        return inventory.getCountSeed();
    }

    public void setUseSeed()
    {
        CanUseSeed = true;
        //Debug.Log("CanUseSeed가뭘까" + CanUseSeed);
    }

    GameObject TreeToMake() //여기 수정
    {
        //Debug.Log("Tree to make 호출됨");
        //return GameObject.FindObjectOfType<Seed>().selectTree();

        int randomItemCount = UnityEngine.Random.Range(0, trees.Length); //?
        GameObject selectedPrefab = trees[randomItemCount];
        return selectedPrefab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FightingZonePlane")
            isInsideZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "FightingZonePlane")
            isInsideZone = false;

    }

    public void makeTree()
    {
        //BoxCollider makeTreeSize = GameObject.FindWithTag("FightingZone").GetComponent<FightingZone>().getBoxCollider();
        //float range_X = makeTreeSize.bounds.size.x;
        //float range_Z = makeTreeSize.bounds.size.z;
        //Debug.Log("make tree 호출");

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("make tree 호출");

            //Debug.Log("Can use seed" + CanUseSeed);
            if(CanUseSeed == true) //슬롯에서 씨앗을 사용하면 심을 수 있게 만듬
            {
                if(isInsideZone == true) //fighting zone 안에서만 나무 심을 수 있음
                {
                    seedCount = setSeedCount();
                    //Debug.Log("seed Count 갯수 " + seedCount);
                    if (seedCount > -1)
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
                        //inventory.setSeedCount();
                        //Debug.Log("Tree Point : " + treePoint);
                        //Instantiate(tree, treePoint, Quaternion.identity);

                        //Debug.Log("Can Use seed" + CanUseSeed);
                        CanUseSeed = false;
                        //Debug.Log("Can Use seed" + CanUseSeed);
                    }
                }
            }
        }
        else
            return;
    }

}
