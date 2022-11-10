using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //애니메이터 조작을 위한 변수
    private bool hitted;

    //체력 바 조작을 위한 변수
    public Slider hpbar;
    public float EMaxHealth;
    private float ECurrentHealth;

    //총알
    public GameObject weaponFactory;
    public GameObject weaponPosition;

    Vector3 toPlayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //player_pos = GameObject.FindWithTag("Player").GetComponent<Player>().getPos();
        ECurrentHealth = EMaxHealth;
        Movespeed = 2.0f;
        hitted = false;
        anim.SetBool("isDie", false);
    }

    // Update is called once per frame
    void Update()
    {
        HPBarControl();
        LookPlayer();
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
            Destroy(other.gameObject); //총알 삭제
            //CurrentHealth -= damage;// 총알 한대에 1만큼 체력이 닳음
            ECurrentHealth = ECurrentHealth - damage;

            //Debug.Log("Current Health " + ECurrentHealth);

            if (ECurrentHealth < 0) //체력이 0이 되면 적 삭제
            {
                //died = true;
                anim.SetBool("isDie", true);
                Destroy(gameObject, DestroyTime);
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Fire();
            StartCoroutine(AttackDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetBool("isDance", false);
            anim.SetBool("isCloser", false);
        }
    }
    IEnumerator GetHit()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        hitted = false;
        anim.SetBool("isAttacked", hitted);
    }

    IEnumerator AttackDelay()
    {
        float delayTime = 5f;
        yield return new WaitForSeconds(delayTime);
    }

    void Fire()
    {
        GameObject weapon = Instantiate(weaponFactory);
        weapon.transform.position = weaponPosition.transform.position;
        //StartCoroutine(AttackDelay());
    }

    public Vector3 GetToPlayer()
    {
        return gameObject.transform.forward;
    }

}
