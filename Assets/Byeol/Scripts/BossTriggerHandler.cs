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

            if (BossControl.playerIsInFightingZone)
            {
                if (!alreadySavedFirstMeetPos)
                {
                    BossControl.firstMetPos = other.transform.position;
                    Debug.Log("first met pos" + BossControl.firstMetPos);
                    alreadySavedFirstMeetPos = true;
                }
            }
        }

        //boss랑 player가 싸움터로 이동하고 나서만
        if (BossIsFightingWithPlayer)
        {
            if (other.gameObject.tag.Equals("Bullet"))
            {
                //Destroy(other.gameObject);
                FindObjectOfType<bulletPool>().ReturnBullet(other.gameObject);

                //existingBoss.GetAnimator().SetBool("bossGetHit", true);

                float damage = 
                    GameObject.FindWithTag("Player").GetComponent<Player>().getPower();
                existingBoss.TakeDamage(damage);

            }
        }
    }
}
