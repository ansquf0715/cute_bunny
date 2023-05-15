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
            }
        }

        if(collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("ÃÑ¸Â¾Ò´Ù À¸¾Ç");
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
