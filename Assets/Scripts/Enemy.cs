using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    
    Animator anim;
    public GameObject Player;
    private float Dist;
    private float DestroyTime = 1.0f;
    private float damage;

    //�ִϸ����� ������ ���� ����
    private bool hitted;

    //ü�� �� ������ ���� ����
    public Slider hpbar;
    public float MaxHealth;
    private float CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        CurrentHealth = MaxHealth;
        hitted = false;
        anim.SetBool("isDie", false);
    }

    // Update is called once per frame
    void Update()
    {
        HPBar_Control();
    }

    private void OnCollisionEnter(Collision other)
    {
        damage = GameObject.FindWithTag("Player").GetComponent<Player>().getDamage();

        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject); //�Ѿ� ����
            //CurrentHealth -= damage;// �Ѿ� �Ѵ뿡 1��ŭ ü���� ����
            CurrentHealth = CurrentHealth - damage;

            Debug.Log("Current Health " + CurrentHealth);

            if (CurrentHealth < 0) //ü���� 0�� �Ǹ� �� ����
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

    void HPBar_Control()
    {
        hpbar.value = CurrentHealth / MaxHealth;
    }

    //public void Enemy_Rotation()
    //{
    //    Quaternion rot = gameObject.transform.rotation;

    //    rot = Quaternion.LookRotation
    //        (Player.transform.position - this.gameObject.transform.position);
    //    Debug.Log("Rot : " + rot);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isDance", true);
            anim.SetBool("isCloser", true);
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
}
