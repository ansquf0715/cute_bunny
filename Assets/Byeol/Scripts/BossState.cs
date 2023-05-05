using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public abstract class BossState
    {
        virtual public void start(Boss boss, Transform player) { }
        virtual public BossState handleInput(Boss boss, Transform player) { return null; }
        virtual public void update(Boss boss, Transform player) { }
        virtual public void end(Boss boss, Transform player) { }
    }

    public class StrollState:BossState
    {
        GameObject bossPlane;
        Collider[] cols = new Collider[3];
        bool playerIsInFightingZone;
        Vector3 randomPos;

        public override void start(Boss boss, Transform player)
        {
            Debug.Log("stroll state start");

            bossPlane = GameObject.Find("BossP");
            cols = bossPlane.GetComponentsInChildren<Collider>();
            playerIsInFightingZone = false;

            boss.GetAnimator().Play("WalkFWD");
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            playerIsInFightingZone = BossControl.playerIsInFightingZone;
            if(playerIsInFightingZone)
            {
                return new WalkState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            if (!boss.NavMeshAgent.pathPending && boss.NavMeshAgent.remainingDistance < 0.5f)
            {
                Debug.Log("stroll update");
                setRandomPos();
                boss.NavMeshAgent.SetDestination(randomPos);
            }
        }

        void setRandomPos()
        {
            int randomCol = Random.Range(0, 3);
            Bounds bounds;
            Vector3 pos;

            switch(randomCol)
            {
                case 0: //0¹ø col
                    bounds = cols[0].bounds;
                    randomPos = new Vector3(
                        Random.Range(bounds.min.x, bounds.max.x),
                        0,
                        Random.Range(bounds.min.z, bounds.max.z));
                    break;
                case 1:
                    bounds = cols[1].bounds;
                    randomPos = new Vector3(
                        Random.Range(bounds.min.x, bounds.max.x),
                        0,
                        Random.Range(bounds.min.z, bounds.max.z));
                    break;
                case 2:
                    bounds = cols[2].bounds;
                    randomPos = new Vector3(
                        Random.Range(bounds.min.x, bounds.max.x),
                        0,
                        Random.Range(bounds.min.z, bounds.max.z));
                    break;
                default:
                    randomPos = Vector3.zero;
                    break;
            }
        }
    }

    public class WalkState:BossState
    {
        float distance;
        Vector3 destPos;
        Vector3 lastPlayerPos;

        public override void start(Boss boss, Transform player)
        {
            Debug.Log("walk state start");
            //boss.transform.rotation = Quaternion.LookRotation(player.position);
            destPos = boss.transform.position;
            lastPlayerPos = player.position;
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(boss.transform.position, player.transform.position);
            if (distance < 20f)
            {
                return new RunState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            if(player.position != lastPlayerPos)
            {
                destPos = player.position;
                lastPlayerPos = player.position;
            }
            boss.NavMeshAgent.SetDestination(destPos);
        }
    }

    public class RunState:BossState
    {
        float distance;
        Vector3 destPos;
        Vector3 lastPlayerPos;

        public override void start(Boss boss, Transform player)
        {
            Debug.Log("run state start");
            destPos = boss.transform.position;
            lastPlayerPos = player.position;
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(player.position, boss.transform.position);
            if (distance < 20f)
            {

            }
            if (distance >= 20f)
            {
                return new WalkState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            if(player.position != lastPlayerPos)
            {
                destPos = player.position;
                lastPlayerPos = player.position;
            }

            boss.NavMeshAgent.SetDestination(destPos);
            boss.NavMeshAgent.speed = 7f;
        }
    }

    public class AttackState:BossState
    {
        float distance;

        public override void start(Boss boss, Transform player)
        {
            Debug.Log("attack state start");
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(player.position, boss.transform.position);
            if(distance >= 5f)
            {
                return new RunState();
            }

            if(boss.Hp <= 3f)
            {
                return new FleeState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {

        }
    }

    public class FleeState:BossState
    {
        float step;

        public override void start(Boss boss, Transform player)
        {

        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            if(boss.Hp >= 6)
            {
                return new WalkState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            Vector3 dir = boss.transform.position - player.position;
            Vector3 destPos = boss.transform.position + dir.normalized * 10f;
            boss.NavMeshAgent.SetDestination(destPos);

            if(boss.NavMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                Vector2 randomCircle = Random.insideUnitCircle * 10f;
                Vector3 randomDest = boss.transform.position
                    + new Vector3(randomCircle.x, 0f, randomCircle.y);
                boss.NavMeshAgent.SetDestination(randomDest);
            }
        }
    }
}
