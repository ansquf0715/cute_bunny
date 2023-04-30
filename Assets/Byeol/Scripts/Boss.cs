using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    GameObject moveParticle;

    public bool playerIsInFightingZone = false;

    public float obstacleDetectionRadius = 1.0f;

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

        agent.SetDestination(setRandomPos());
    }

    // Update is called once per frame
    void Update()
    {
        findToGo();
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
        if(other.gameObject.tag.Equals("Player"))
        {
            //Debug.Log(" meet player");
            agent.isStopped = true;
            anim.SetBool("meetPlayer", true);
        }
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
        Debug.Log("find to go");
        if(playerIsInFightingZone) //player가 fightingZone안에 있으면
        {
            Debug.Log("player is in fighting zone");
            agent.SetDestination(Player.playerPos);
        }
        else
        {
            if (agent.remainingDistance < 1f && !agent.pathPending)
            {
                Debug.Log("player is not in fighting zone");
                agent.SetDestination(setRandomPos());
            }
        }
    }

}
