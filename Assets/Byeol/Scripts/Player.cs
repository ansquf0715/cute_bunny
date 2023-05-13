using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    static public Vector3 playerPos;

    private Text seedInfo;

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
    static public float CurrentHealth; //현재 체력
    

    Vector3 moveVec;
    Animator anim;
    Rigidbody rigid;

    int seedCount; //씨앗 개수
    bool CanUseSeed = false; //인벤토리에서 사용되면

    //나무를 시간을 두고 심기 위해서
    List<GameObject> madeTrees = new List<GameObject>();
    List<Vector3> madeTreePos = new List<Vector3>();
    List<int> growDays = new List<int>();
    List<GameObject> treeHoles = new List<GameObject>();

    public GameObject[] trees;
    public GameObject treeHole;

    BoxCollider fightingZoneCollider;
    bool isInsideZone = false;

    bool playerDied = false;

    int plantedTreeCount = 0;
    bool checkPlantTreeQuest = false; //for heart quest
    bool checkBirdTreeQuest = false; //for bird quest

    // Start is called before the first frame update
    void Start()
    {
        seedInfo = GameObject.Find("SeedInfo").GetComponent<Text>();

        inventory = FindObjectOfType<Inventory>();
        anim = GetComponentInChildren<Animator>();
        fightingZoneCollider = GameObject.FindWithTag("FightingZonePlane").GetComponent<BoxCollider>();

        anim.SetBool("isAttacking", false);
        anim.SetBool("isDie", false);

        rigid = GetComponent<Rigidbody>();
        power = 2.0f;
        MaxHealth = 20.0f;
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = this.transform.position;

        if(playerDied == false)
        {
            PlayerMove();
            Fire();
            rigid.velocity = Vector3.zero;
            
            if (inventory.getCountSeed() > -1)
            {
                makeTree();
            }
            if (CurrentHealth <= 0)
            {
                checkPlayerHP();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("isAttacking", true);
            Vector3 spawnPos = transform.position;
            spawnPos.x = transform.position.x + 1f;
            spawnPos.y = transform.position.y + 3f;
            GameObject carrot = Instantiate(bulletFactory,
                spawnPos, Quaternion.identity);
            Rigidbody carrotRb = carrot.GetComponent<Rigidbody>();
            carrotRb.useGravity = true;
            float carrotSpeed = 10f;
            carrotRb.AddForce(transform.forward * carrotSpeed,
                ForceMode.Impulse);

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

    public void setHealth(float newHealth) //temp 사용 -> 고쳐야될듯
    {
        if (newHealth <= 0)
        {
            anim.SetTrigger("isHit");
        }
        CurrentHealth += newHealth;
        Debug.Log("current health" + CurrentHealth);
        FindObjectOfType<HPBarControl>().isChange();
    }

    public void minusPlayerHealth(float newHealth)
    {
        CurrentHealth -= newHealth;
        Debug.Log("current health" + CurrentHealth);
        FindObjectOfType<HPBarControl>().isChange();
    }

    public void checkPlayerHP() //체력을 확인하고 0되면 애니메이션 나오게하기
    {
        anim.SetBool("isDie", true);
        playerDied = true;
        StartCoroutine(Die());
        StartCoroutine(DelayMove());
    }

    IEnumerator DelayMove()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("isDie", false);
        playerDied = false;
        CurrentHealth = MaxHealth;
        FindObjectOfType<HPBarControl>().isChange();
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
    }

    IEnumerator rebirth()
    {
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
        return inventory.getCountSeed();
    }

    public void setUseSeed()
    {
        CanUseSeed = true;
    }

    GameObject TreeToMake() //여기 수정
    {
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

    void saveTreeInformation(Vector3 treePoint)
    {
        madeTrees.Add(TreeToMake());
        madeTreePos.Add(treePoint);
        growDays.Add(0);
        GameObject hole = Instantiate(treeHole, treePoint, Quaternion.identity);
        treeHoles.Add(hole);
    }

    void appearTree()
    {
        //heart Quest 5 : plant a tree
        plantedTreeCount++;
        //Debug.Log("planted tree count" + plantedTreeCount);
        if (plantedTreeCount >= 1 && !checkPlantTreeQuest)
        {
            //Debug.Log("check plant tree quest");
            FindObjectOfType<QuestManager>().plantedTreePlus();
            checkPlantTreeQuest = true;
            plantedTreeCount = 0;
        }
        if(plantedTreeCount >= 1 && !checkBirdTreeQuest)
        {
            Debug.Log("check plant bird quest");
            FindObjectOfType<QuestManager>().plantedTreeForBird();
            checkBirdTreeQuest = true;
            plantedTreeCount = 0;
        }

        Destroy(treeHoles[0]);
        Instantiate(madeTrees[0], madeTreePos[0], Quaternion.identity);
        madeTrees.RemoveAt(0);
        madeTreePos.RemoveAt(0);
        treeHoles.RemoveAt(0);
    }

    public void makeTree()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (CanUseSeed == true && isInsideZone == true)
            {
                seedCount = setSeedCount();
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

                    saveTreeInformation(treePoint);
                    Invoke("appearTree", 3f);

                    CanUseSeed = false;
                    seedInfo.text = " ";

                }

            }
        }
    }
}
