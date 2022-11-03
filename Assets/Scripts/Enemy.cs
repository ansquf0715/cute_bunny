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

    //�ִϸ����� ������ ���� ����
    //private bool died;
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
        //died = false;
        hitted = false;
        anim.SetBool("isDie", false);
    }

    // Update is called once per frame
    void Update()
    {
        HPBar_Control();
    }
    //void MoveAnim()
    //{
    //    //player�� enemy�� �Ÿ��� 10.0f���� ���� ��� ������ �ڼ��� ����

    //    Dist = Vector3.Distance(Player.transform.position, gameObject.transform.position);
    //    if (Dist < 10.0f) //player�� �Ÿ� �ȿ� ������
    //    {
    //        anim.SetBool("isDance", true);
    //        anim.SetBool("isCloser", closer);
    //        anim.SetBool("isAttacked", hitted);
    //        StartCoroutine(GetHit());
    //    }
    //    else
    //    {
    //        closer = false;
    //        anim.SetBool("isDance", false);
    //        anim.SetBool("isCloser", closer);
    //        anim.SetBool("isAttacked", hitted);
    //    }

    //}
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject); //�Ѿ� ����
            CurrentHealth--; // �Ѿ� �Ѵ뿡 1��ŭ ü���� ����

            if (CurrentHealth == 0) //ü���� 0�� �Ǹ� �� ����
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
