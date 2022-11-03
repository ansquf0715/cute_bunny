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

    //애니메이터 조작을 위한 변수
    //private bool died;
    private bool hitted;

    //체력 바 조작을 위한 변수
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
    //    //player와 enemy의 거리가 10.0f보다 작은 경우 공격할 자세를 취함

    //    Dist = Vector3.Distance(Player.transform.position, gameObject.transform.position);
    //    if (Dist < 10.0f) //player가 거리 안에 들어오면
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
            Destroy(other.gameObject); //총알 삭제
            CurrentHealth--; // 총알 한대에 1만큼 체력이 닳음

            if (CurrentHealth == 0) //체력이 0이 되면 적 삭제
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
