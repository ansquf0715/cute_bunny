using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Enemy : MonoBehaviour
{
    
    Animator anim;
    //public GameObject Player;
    //private Transform Player_pos;
    private Vector3 player_pos;
    private float Dist;
    private float DestroyTime = 1.0f;
    private float damage;
    private float Movespeed;
    private bool checkInside = false;
    //private bool keepAttacking = false;

    //�ִϸ����� ������ ���� ����
    private bool hitted;

    //ü�� �� ������ ���� ����
    public Slider hpbar;
    public float EMaxHealth;
    private float ECurrentHealth;

    //�Ѿ�
    public GameObject weaponFactory;
    public GameObject weaponPosition;

    Vector3 toPlayer;

    public GameObject[] items; //enemy�� óġ�ϸ� ����� �����۵�
    BoxCollider rangeCollider; //������ ����Ʈ�� ��ġ ������ ���� ����

    bool changeDiedEnemyCount = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //player_pos = GameObject.FindWithTag("Player").GetComponent<Player>().getPos();
        ECurrentHealth = EMaxHealth;
        Movespeed = 2.0f;
        hitted = false;
        anim.SetBool("isDie", false);
        rangeCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckPlayerHealth();
        

        if(GameObject.FindWithTag("Player").GetComponent<Player>().checkDie() == false)
        {
            HPBarControl();
            LookPlayer();
        }

        if (GameObject.FindWithTag("Player").GetComponent<Player>().checkDie() == true)
            Destroy(this.gameObject);

        if (ECurrentHealth < 0f)
        {
            if (!changeDiedEnemyCount)
            {
                //FindObjectOfType<QuestManager>().diedEnemyCount++;
                FindObjectOfType<QuestManager>().diedEnemyPlus();
                changeDiedEnemyCount = true;
            }
        }
    }

    void LookPlayer()
    {
        //this.transform.LookAt(Player_pos);

        player_pos = GameObject.FindWithTag("Player").GetComponent<Player>().getPos();
        this.transform.position = Vector3.MoveTowards(this.transform.position,
            player_pos, Movespeed * Time.deltaTime);

        toPlayer = player_pos - transform.position;
        transform.rotation = Quaternion.LookRotation(toPlayer).normalized;
    }

    private void OnCollisionEnter(Collision other)
    {
        damage = GameObject.FindWithTag("Player").GetComponent<Player>().getPower();

        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject); //�Ѿ� ����
            //CurrentHealth -= damage;// �Ѿ� �Ѵ뿡 1��ŭ ü���� ����
            ECurrentHealth = ECurrentHealth - damage;

            //Debug.Log("Current Health " + ECurrentHealth);

            if (ECurrentHealth < 0) //ü���� 0�� �Ǹ� �� ����
            {
                //died = true;
                anim.SetBool("isDie", true);

                Destroy(gameObject, DestroyTime);
                dropItem();
            }

            hitted = true;
            anim.SetBool("isAttacked", hitted);
            StartCoroutine(GetHit());

        }
    }

    void HPBarControl()
    {
        hpbar.value = ECurrentHealth / EMaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isDance", true);
            anim.SetBool("isCloser", true);

            //if (keepAttacking == true)
            //{
            //    Debug.Log("Invoke ");
            //    InvokeRepeating("Fire", 0, 1);
            //}

            checkInside = true;
            if(checkInside == true)
            {
                InvokeRepeating("Fire", 0, 1);
            }
        }
    }

    private void CheckPlayerHealth()
    {
        //Debug.Log("Check Player Health");
        if (GameObject.FindWithTag("Player").GetComponent<Player>().checkDie() == true)
        {
            //keepAttacking = false;
            //Debug.Log("keep attacking " + keepAttacking);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        //InvokeRepeating("Fire", 0, 3);
    //        //Invoke("Fire", 3);
    //        //Fire();
    //        //StartCoroutine(AttackDelay());
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetBool("isDance", false);
            anim.SetBool("isCloser", false);
            checkInside = false;
            CancelInvoke();
        }
    }
    IEnumerator GetHit()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        hitted = false;
        anim.SetBool("isAttacked", hitted);
    }

    //IEnumerator AttackDelay()
    //{
    //    //Debug.Log("attack delay ȣ��");
    //    float delayTime = 5f;
    //    yield return new WaitForSeconds(delayTime);
    //}

    void Fire()
    {
        //Debug.Log("Fire �ҷ���");
        GameObject weapon = Instantiate(weaponFactory);
        //Thread.Sleep(2000);
        //StartCoroutine(AttackDelay());

        weapon.transform.position = weaponPosition.transform.position;
    }

    public Vector3 GetToPlayer()
    {
        return gameObject.transform.forward;
    }

    public Vector3 GetEnemyPos()
    {
        return this.gameObject.transform.position;
    }

    //int RandomItemCount() //�� ���� ����Ʈ�� �� ���� ���ϱ�
    //{
    //    int randomItemCount = Random.Range(0, 3);
    //    Debug.Log("random Item Count" + randomItemCount);
    //    return randomItemCount;
    //}

    GameObject selectItem() //�������� ����Ʈ�� ������ ���ϱ�
    {
        int selectedIndex = Random.Range(0, items.Length);
        GameObject selectedPrefab = items[selectedIndex];
        return selectedPrefab;
    }

    void dropItem() //������ ����Ʈ����!
    {
        int randomItemCount = Random.Range(0, 2);

        for (int i = 0; i < randomItemCount; i++)
        {
            Instantiate(selectItem(), RandomItemPosition(), Quaternion.identity);
        }
    }

    Vector3 RandomItemPosition() //�������� ����Ʈ�� ������ ��ġ
    {
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);

        Vector3 RandomPosition = new Vector3(range_X, 1f, range_Z);
        Vector3 SpawnPos = gameObject.transform.position + RandomPosition;

        //Debug.Log("Spawn Pos" + SpawnPos);
        return SpawnPos;
    }
}
