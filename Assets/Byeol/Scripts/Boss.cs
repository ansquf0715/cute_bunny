using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    NavMeshAgent agent;
    Rigidbody rigid;

    Transform target;

    GameObject fightingZone;
    BoxCollider fightingZoneCollider;
    Bounds fightingZoneBound;

    public Collider[] cols = new Collider[3];

    Collider boxCol;
    Animator anim;

    public GameObject particle;
    ParticleSystem moveParticle;
    ParticleSystem clonedMoveParticle;
    Vector3 particlePos;

    Image useBlackImage;

    public bool playerIsInFightingZone = false;
    public float obstacleDetectionRadius = 1.0f;

    public bool bossIsMoved = false;
    public bool bossIsInBossZone = false;

    public GameObject bossPlane;

    public bool alreadyCheckDest = false;

    Slider bossHealthSlider;
    int randomAnimNumber;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();

        fightingZone = GameObject.Find("FightingZonePlane");
        fightingZoneCollider = fightingZone.GetComponent<BoxCollider>();
        fightingZoneBound = fightingZoneCollider.bounds;

        GameObject bossP = GameObject.Find("BossP");
        cols = bossP.GetComponentsInChildren<Collider>();

        boxCol = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();

        moveParticle = particle.GetComponent<ParticleSystem>();

        Canvas canv = GameObject.Find("Canvas").GetComponent<Canvas>();
        useBlackImage = canv.transform.Find("coverBlack").GetComponent<Image>();

        bossPlane = GameObject.Find("BossPlane");

        bossHealthSlider = canv.transform.Find("BossBar").GetComponent<Slider>();

        agent.SetDestination(setRandomPos());
    }

    // Update is called once per frame
    void Update()
    {
        findToGo();
        checkBossWalking();
        if (bossIsInBossZone)
        {
            MoveToPlayer();
        }
    }

    void checkBossWalking()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, Player.playerPos);

        //player와 boss 사이의 거리가 20이하이면 run
        if(distanceToPlayer <= 20f)
        {
            anim.SetBool("bossRun", true);
            anim.SetBool("bossWalking", false);
        }
        else
        {
            anim.SetBool("bossWalking", true);
            anim.SetBool("bossRun", false);
        }

        //boss가 걸어가고 있을 때
        if (agent.velocity.magnitude > 0f)
        {
            anim.SetBool("bossWalking", true);
        }
        else //boss가 멈춰있을 때
        {
            anim.SetBool("bossWalking", false);
        }
    }

    private void FixedUpdate()
    {
        if(!fightingZoneCollider.bounds.Contains(Player.playerPos))
        {
            playerIsInFightingZone = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!bossIsMoved)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                agent.isStopped = true;
                anim.SetBool("meetPlayer", true);

                particlePos.x = this.transform.position.x;
                particlePos.y = this.transform.position.y;
                particlePos.z = this.transform.position.z - 3f;
                //particlePos = this.transform.position;

                clonedMoveParticle = Instantiate(moveParticle, particlePos, Quaternion.identity);
                clonedMoveParticle.Play();

                Invoke("destroyParticle", 3f);
            }
        }
    }

    void destroyParticle()
    {
        Debug.Log("destroy particle");
        clonedMoveParticle.Stop();

        StartCoroutine(ChangeImageHeight());
    }

    IEnumerator ChangeImageHeight()
    {
        useBlackImage.gameObject.SetActive(true);
        useBlackImage.color = new Color(useBlackImage.color.r,
            useBlackImage.color.g, useBlackImage.color.b, 0f);

        float startHeight = useBlackImage.rectTransform.sizeDelta.y;
        float elapsedTime = 0;
        float targetHeight = 1200f;
        float duration = 1f;

        while(elapsedTime < duration)
        {
            float newHeight = Mathf.Lerp(startHeight, targetHeight, elapsedTime / duration);
            useBlackImage.rectTransform.sizeDelta =
                new Vector2(useBlackImage.rectTransform.sizeDelta.x, newHeight);

            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            useBlackImage.color = new Color(useBlackImage.color.r,
                useBlackImage.color.g, useBlackImage.color.b, newAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        useBlackImage.rectTransform.sizeDelta =
            new Vector2(useBlackImage.rectTransform.sizeDelta.x, targetHeight);
        useBlackImage.color = new Color(useBlackImage.color.r,
            useBlackImage.color.g, useBlackImage.color.b, 1f);

        bossIsMoved = true;
        agent.enabled = false;
        this.transform.position = new Vector3(
            -164f, 0, 61f);
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(-166f, 0, 35f);

        useBlackImage.gameObject.SetActive(false);

        bossHealthSlider.gameObject.SetActive(true);

        anim.SetBool("meetPlayer", false);

        Bounds bossPlaneBounds = bossPlane.GetComponent<Collider>().bounds;
        if (bossPlaneBounds.Contains(this.transform.position))
            bossIsInBossZone = true;

        yield return new WaitForSeconds(0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "FightingZonePlane"
            && other.bounds.Contains(Player.playerPos))
        {
            playerIsInFightingZone = true;
        }
    }

    Vector3 setRandomPos() //col 3개 중 한 군데 랜덤으로 정해서 그 안에서 랜덤 포지션
    {
        int randomCol = Random.Range(0, 3);
        Bounds bounds;
        Vector3 randomPos;

        switch(randomCol)
        {
            case 0: //0번 col
                bounds = cols[0].bounds;
                randomPos = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    0,
                    Random.Range(bounds.min.z, bounds.max.z));
                Debug.Log("random Pos col 0" + randomPos);
                return randomPos;
            case 1: //1번 col
                bounds = cols[1].bounds;
                randomPos = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    0,
                    Random.Range(bounds.min.z, bounds.max.z));
                Debug.Log("random Pos col 1" + randomPos);
                return randomPos;
            case 2: //2번 col
                bounds = cols[2].bounds;
                randomPos = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    0,
                    Random.Range(bounds.min.z, bounds.max.z));
                Debug.Log("random Pos col 2" + randomPos);
                return randomPos;
            default:
                return Vector3.zero;
        }
    }

    void findToGo()
    {
        if(!bossIsMoved)
        {
            if (playerIsInFightingZone) //player가 fightingZone안에 있으면
            {
                //Debug.Log("player is in fighting zone");
                agent.SetDestination(Player.playerPos);
            }
            else
            {
                if (agent.remainingDistance < 1f && !agent.pathPending)
                {
                    //Debug.Log("player is not in fighting zone");
                    agent.SetDestination(setRandomPos());
                }
            }
        }
    }

    void MoveToPlayer()
    {
        if (!alreadyCheckDest)
        {
            alreadyCheckDest = true;
            Debug.Log("move to player");
            agent.enabled = true;
            agent.SetDestination(Player.playerPos);
        }
        else
        {
            agent.SetDestination(Player.playerPos);
        }

        ControlFightingAnim();
    }

    void ControlFightingAnim()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, Player.playerPos);
        //player와 boss 사이의 거리가 10이하이면 attack
        int randomNumber = Random.Range(0, 3);
        Debug.Log("random Number" + randomNumber);
        if (distanceToPlayer <= 10f)
        {
            setRandomAttackMotion(randomNumber);
        }
        else
        {
            exitAttackMotion(randomNumber);
        }
    }


    void setRandomAttackMotion(int randomNum)
    {
        switch (randomNum)
        {
            case 0:
                anim.SetBool("bossAttack1", true);
                break;
            case 1:
                anim.SetBool("bossAttack2", true);
                break;
            case 2:
                anim.SetBool("bossAttack3", true);
                break;
        }
    }

    void exitAttackMotion(int randomNum)
    {
        switch (randomNum)
        {
            case 0:
                anim.SetBool("bossAttack1", false);
                break;
            case 1:
                anim.SetBool("bossAttack2", false);
                break;
            case 2:
                anim.SetBool("bossAttack3", false);
                break;
        }
    }

}
