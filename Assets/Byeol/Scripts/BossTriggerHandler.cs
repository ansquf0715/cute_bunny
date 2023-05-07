using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;

public class BossTriggerHandler : MonoBehaviour
{
    bool BossIsFightingWithPlayer;
    bool alreadySavedFirstMeetPos;

    // Start is called before the first frame update
    void Start()
    {
        BossIsFightingWithPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        BossIsFightingWithPlayer = Boss.bossIsMoved;
        alreadySavedFirstMeetPos = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Boss existingBoss = Boss.GetInstance();

        if (other.gameObject.tag.Equals("Player"))
        {
            if(!alreadySavedFirstMeetPos)
            {
                alreadySavedFirstMeetPos = true;
                Boss.meetPlayerPos = other.transform.position;
            }

            //BossState currentState = existingBoss.GetCurrentState();
            //if (currentState.GetType() == typeof(AttackState))
            //{
            //    GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(-2);
            //    Debug.Log("get health" + 
            //        GameObject.FindWithTag("Player").GetComponent<Player>().getHealth());
            //}
        }

        //boss랑 player가 싸움터로 이동하고 나서만
        if (BossIsFightingWithPlayer)
        {
            if (other.gameObject.tag.Equals("Bullet"))
            {
                Destroy(other.gameObject);

                float damage = 
                    GameObject.FindWithTag("Player").GetComponent<Player>().getPower();
                existingBoss.TakeDamage(damage);

            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Boss existingBoss = Boss.GetInstance();

    //    if (other.gameObject.tag.Equals("Player"))
    //    {
    //        Debug.Log("boss & player");
    //        //boss가 현재 공격중이면
    //        BossState currentState = existingBoss.GetCurrentState();
    //        if (currentState.GetType() == typeof(AttackState))
    //        {
    //            GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(-2);
    //        }
    //    }
    //}
}
