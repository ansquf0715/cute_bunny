using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using StatePattern;

public class BabyBoss : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    Animator animator;

    float health = 5;

    Slider healthBar;
    float maxHealth;
    float currentHealth;

    float speed;

    int attackCount = 0;

    AudioClip punchSound;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        maxHealth = 5f;
        currentHealth = maxHealth;

        Canvas can = GetComponentInChildren<Canvas>();
        healthBar = can.GetComponentInChildren<Slider>();
        healthBar.value = maxHealth;

        speed = agent.speed;
        //Debug.Log("speed" + speed);

        audio = GetComponent<AudioSource>();
        if (this.gameObject.tag.Equals("babyBoss1"))
        {
            punchSound = Resources.Load<AudioClip>("babyBoss1Punch");
        }
        else if(this.gameObject.tag.Equals("babyBoss2"))
        {
            punchSound = Resources.Load<AudioClip>("babyBoss2Punch");
        }
        audio.clip = punchSound;
        audio.volume = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.GetComponent<Player>().checkDie())
        {
            HpBarControl();
            if(currentHealth > 0f)
            {
                SetDestinationToPlayer();
            }
            run();
            checkPlayerDistance();
        }
    }

    void SetDestinationToPlayer()
    {
        Vector3 direciton = player.transform.position - this.gameObject.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direciton);
        this.transform.rotation = rotation;
    }

    void run()
    {
        float distance = Vector3.Distance(this.gameObject.transform.position,
            player.transform.position);

        if(distance >= 15f)
        {
            agent.speed = 3.5f;
            if(animator.GetBool("run") == true)
                animator.SetBool("run", false);

            animator.SetBool("walk", true);
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
        else if (distance <= 15f)
        {
            //Debug.Log("run");
            //animator.SetBool("walk", false);
            animator.SetBool("run", true);
            agent.speed *= 2;
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
    }

    void checkPlayerDistance()
    {
        float distance = Vector3.Distance(this.gameObject.transform.position,
            player.transform.position);

        if(distance < 8f)
        {
            //Debug.Log("player is close to me");
            animator.SetBool("attack", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(animator.GetBool("attack"))
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                player.GetComponent<Player>().setHealth(-1f);
                if(this.gameObject.tag.Equals("babyBoss1")) //1번 baby boss이면
                {
                    GameObject punch1 = Resources.Load<GameObject>("babyBossPunch1");
                    GameObject clonedPunch1 = Instantiate(
                        punch1, this.gameObject.transform.position, Quaternion.identity);
                    Destroy(clonedPunch1, 1f);

                    Vector3 collisionPoint = 
                        collision.gameObject.GetComponent<Collider>().ClosestPoint(player.transform.position);
                    GameObject attack1 = Resources.Load<GameObject>("babyBossAttack1");
                    GameObject clonedAttack1 = Instantiate(
                        attack1, collisionPoint, Quaternion.identity);
                    Destroy(clonedAttack1, 1f);
                    
                    audio.Play();
                }
                else if(this.gameObject.tag.Equals("babyBoss2")) //2번 baby boss이면
                {
                    GameObject punch2 = Resources.Load<GameObject>("babyBossPunch2");
                    GameObject clonedPunch2 = Instantiate(
                        punch2, this.gameObject.transform.position, Quaternion.identity);
                    Destroy(clonedPunch2, 1f);

                    Vector3 collisionPoint =
                        collision.gameObject.GetComponent<Collider>().ClosestPoint(player.transform.position);
                    collisionPoint.y += 3;
                    GameObject attack2 = Resources.Load<GameObject>("babyBossAttack2");
                    GameObject clonedAttack2 = Instantiate(
                        attack2, collisionPoint, Quaternion.identity);
                    Debug.Log("attack2 " + clonedAttack2);
                    Destroy(clonedAttack2, 1f);
                    
                    audio.Play();               }
            }
        }

        if(collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("총맞았다 으악");
            animator.SetTrigger("getHit");
            currentHealth -= player.GetComponent<Player>().getPower();

            if(currentHealth <= 0f)
            {
                agent.isStopped = true;
                animator.SetTrigger("die");
                //FindObjectOfType<BossControl>().diedBabyBoss++;
                BossControl.diedBabyBoss++;
                Debug.Log("died baby boss count" + BossControl.diedBabyBoss);

                Destroy(this.gameObject, 3f);

            }
        }
    }

    void HpBarControl()
    {
        healthBar.value = currentHealth / maxHealth;
    }
}
