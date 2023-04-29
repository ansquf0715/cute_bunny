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

        Vector3 tempTarget = new Vector3(-4f, 0, 38f);
        agent.SetDestination(tempTarget);

        Debug.Log("fightingZoneBound" + fightingZoneBound.min.z);
        Debug.Log("fightingZoneBound" + fightingZoneBound.max.z);

        //rigid.velocity = new Vector3(0, -1, 0);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "FightingZonePlane"
            && other.bounds.Contains(Player.playerPos))
        {
            //Debug.Log("Player is inside the plane");
            playerIsInFightingZone = true;
        }
    }

    //void findToGo()
    //{
    //    if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
    //        return;

    //    NavMeshPath path = new NavMeshPath();

    //    if (agent.CalculatePath(agent.destination, path) && path.status == NavMeshPathStatus.PathComplete)
    //    {
    //        // ���� ��ȿ�� ���
    //        agent.SetDestination(agent.destination);
    //    }
    //    else
    //    {
    //        // ���� ��ȿ���� ���� ���
    //        Vector3 newDestination = GetRandomPointInBounds(fightingZoneBound);

    //        agent.SetDestination(newDestination);
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� NavMeshObstacle�� ���
        if (collision.gameObject.GetComponent<NavMeshObstacle>() != null)
        {
            Vector3 newDestination = GetRandomPointInBounds(fightingZoneBound);

            agent.SetDestination(newDestination);
        }
    }

    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        Vector3 randomPos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z));

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPos, out hit, obstacleDetectionRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return GetRandomPointInBounds(bounds);
        }
    }

    void findToGo()
    {
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            return;

        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(agent.destination, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            Vector3 newDestination = new Vector3(
                Random.Range(fightingZoneBound.min.x, fightingZoneBound.max.x),
                0,
                Random.Range(fightingZoneBound.min.z, fightingZoneBound.max.z));

            agent.SetDestination(newDestination);
        }
        else
        {
            // ���� ��ȿ���� ���� ���
            Vector3 newDestination = new Vector3(
                Random.Range(fightingZoneBound.min.x, fightingZoneBound.max.x),
                0,
                Random.Range(fightingZoneBound.min.z, fightingZoneBound.max.z));

            agent.SetDestination(newDestination);
            Debug.Log("else���� �ɷ�?");

        }
    }

    //void findToGo()
    //{
    //    if (playerIsInFightingZone) //player�� fightingZone�ȿ� ���� ��
    //    {
    //        agent.SetDestination(Player.playerPos);
    //    }
    //    else
    //    {
    //        if (agent.pathStatus == NavMeshPathStatus.PathComplete
    //            && agent.remainingDistance <= agent.stoppingDistance)
    //        {
    //            //�������� ������ ���
    //            Vector3 randomPos = new Vector3(
    //                Random.Range(fightingZoneBound.min.x, fightingZoneBound.max.x),
    //                0,
    //                Random.Range(fightingZoneBound.min.x, fightingZoneBound.max.x));
    //            Debug.Log("random Pos" + randomPos);
    //            agent.SetDestination(randomPos);
    //        }
    //    }
    //}
}
