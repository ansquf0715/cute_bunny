using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            boss.GetAnimator().SetBool("bossWalking", true);
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            playerIsInFightingZone = BossControl.playerIsInFightingZone;
            if (playerIsInFightingZone)
            {
                return new WalkState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            boss.GetAnimator().Play("WalkFWD");
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
                case 0: //0번 col
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
            destPos = boss.transform.position;
            lastPlayerPos = player.position;

            boss.GetAnimator().SetBool("bossWalking", true);
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

    public class RunState : BossState
    {
        float distance;
        Vector3 destPos;
        Vector3 lastPlayerPos;

        bool isReadyToMove;
        float startTime;
        float moveStartTime;

        GameObject stormParticle;
        GameObject clonedStormParticle;

        Image blackImage;
        Slider HealthSlider;

        bool isChangingHeight;
        float changeHeightStartTime;
        float changeHeightDuration;
        float startHeight;
        float targetHeight;

        public override void start(Boss boss, Transform player)
        {
            Debug.Log("run state start");
            destPos = boss.transform.position;
            lastPlayerPos = player.position;
            startTime = Time.time;
            stormParticle = Resources.Load<GameObject>("BossParticle");

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            blackImage = canvas.transform.Find("coverBlack").GetComponent<Image>();
            HealthSlider = canvas.transform.Find("BossBar").GetComponent<Slider>();

            startHeight = blackImage.rectTransform.sizeDelta.y;

            boss.GetAnimator().SetBool("bossRun", true);
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(player.position, boss.transform.position);

            if(Boss.bossIsMoved)
            {
                if (distance < 10f)
                {
                    return new AttackState();
                }
            }
            if (distance >= 20f)
            {
                return new WalkState();
            }

            if (isReadyToMove)
            {
                blackImage.gameObject.SetActive(false);
                HealthSlider.gameObject.SetActive(true);
                Vector3 newPos = new Vector3(-164f, 0, 61f);
                boss.NavMeshAgent.Warp(newPos);
                player.transform.position = new Vector3(-166f, 0, 35f);
                //boss.bossIsMoved = true;
                Boss.bossIsMoved = true;
                return new WalkState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            if (player.position != lastPlayerPos)
            {
                destPos = player.position;
                lastPlayerPos = player.position;
            }

            boss.NavMeshAgent.SetDestination(destPos);
            boss.NavMeshAgent.speed = 7f;

            if (!boss.firstMeetPlayer) //player를 처음 만났을 때
            {
                if (checkPlayerPosToMove(boss, player) && Time.time - startTime > 3f)
                {
                    boss.firstMeetPlayer = true;
                    StartChangingImageHeight();
                }
            }

            if(isChangingHeight)
            {
                float elapseTime = Time.time - changeHeightStartTime;
                float ratio = elapseTime / changeHeightDuration;

                float newHeight = Mathf.Lerp(startHeight, targetHeight, ratio);
                blackImage.rectTransform.sizeDelta = new Vector2(
                    blackImage.rectTransform.sizeDelta.x, newHeight);

                float newAlpha = Mathf.Lerp(0f, 1f, ratio);
                blackImage.color = new Color(blackImage.color.r,
                    blackImage.color.g, blackImage.color.b, newAlpha);

                if(ratio >= 1f)
                {
                    isChangingHeight = false;
                    isReadyToMove = true;
                }
            }
        }

        void StartChangingImageHeight()
        {
            isChangingHeight = true;
            changeHeightStartTime = Time.time;
            startHeight = blackImage.rectTransform.sizeDelta.y;
            targetHeight = 1200f;
            changeHeightDuration = 1f;
            blackImage.gameObject.SetActive(true);
        }

        bool checkPlayerPosToMove(Boss boss, Transform player)
        {
            float dist = Vector3.Distance(player.position, boss.transform.position);
            if (dist <= 10f) //거리가 2보다 작으면
            {
                boss.NavMeshAgent.isStopped = true;
                boss.GetAnimator().SetBool("meetPlayer", true);

                if (clonedStormParticle == null)
                {
                    clonedStormParticle = GameObject.Instantiate(stormParticle,
                        boss.transform.position, Quaternion.identity);
                }

                ParticleSystem stormSystem = clonedStormParticle.GetComponent<ParticleSystem>();
                if (!stormSystem.isPlaying)
                    stormSystem.Play();

                return true;
            }
            return false;
        }

        public override void end(Boss boss, Transform player)
        {
            boss.GetAnimator().SetBool("meetPlayer", false);
            if (clonedStormParticle != null)
                GameObject.Destroy(clonedStormParticle, 1f);
        }
    }

    public class AttackState:BossState
    {
        float distance;
        int randomMotionNumber;
        bool isAttacking;
        bool changeState;

        string randomMotionName;

        bool hasCollided;
        bool isDustParticleCreated;

        GameObject dustParticle;
        GameObject clonedDustParticle;

        public override void start(Boss boss, Transform player)
        {
            Debug.Log("attack state start");
            changeState = false;
            isAttacking = false;

            randomMotionName = getRandomMotionName();

            hasCollided = false;
            isDustParticleCreated = false;

            dustParticle = Resources.Load<GameObject>("smoke");
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(player.position, boss.transform.position);
            if (distance >= 5f)
            {
                boss.GetAnimator().SetBool(randomMotionName, false);
                return new RunState();
            }

            if (boss.Hp <= 3f)
            {
                return new FleeState();
            }

            if(changeState)
            {
                boss.GetAnimator().SetBool(randomMotionName, false);
                return new RunState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            if (!isAttacking)
            {
                //play animation
                boss.GetAnimator().SetBool(randomMotionName, true);
                isAttacking = true;
            }
            else
            {
                if (!IsPlayingAnimation(boss))
                {
                    GameObject.Destroy(clonedDustParticle, 1f);

                    isAttacking = false;
                    changeState = true;
                }
            }

            if (IsPlayingAnimation(boss))
            {
                MeshCollider[] colliders = boss.transform.GetComponentsInChildren<MeshCollider>();
                foreach (MeshCollider collider in colliders)
                {
                    if (player.GetComponent<Collider>().bounds.Intersects(collider.bounds))
                    {
                        if (!hasCollided)
                        {
                            if (clonedDustParticle == null)
                            {
                                Vector3 newPos = boss.transform.position;
                                newPos.x = boss.transform.position.x + 3;
                                newPos.z = boss.transform.position.z + 3;
                                clonedDustParticle = GameObject.Instantiate(dustParticle, newPos, Quaternion.identity);
                            }
                            else
                            {
                                // 이전 프레임에서 생성된 파티클 객체가 있는 경우, 파티클 위치를 업데이트합니다.
                                clonedDustParticle.transform.position = boss.transform.position + new Vector3(3, 0, 3);
                            }

                            ParticleSystem dustSystem = clonedDustParticle.GetComponent<ParticleSystem>();
                            if (!dustSystem.isPlaying)
                                dustSystem.Play();

                            //player.GetComponent<Player>().setHealth(-2);
                            //hasCollided = true;
                        }
                    }
                }
            }
        }

        public bool IsPlayingAnimation(Boss boss)
        {
            return boss.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;
        }

        void getRandomMotionNum()
        {
            randomMotionNumber = Random.Range(0, 3);
        }

        string getRandomMotionName()
        {
            getRandomMotionNum();
            switch (randomMotionNumber)
            {
                case 0:
                    return "bossAttack1";
                case 1:
                    return "bossAttack2";
                case 2:
                    return "bossAttack3";
                default:
                    return null;
            }
        }
    }

    public class FleeState:BossState
    {
        float step;

        public override void start(Boss boss, Transform player)
        {
            Debug.Log("Flee state start");
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
            //Vector3 dir = boss.transform.position - player.position;
            //Vector3 destPos = boss.transform.position + dir.normalized * 10f;
            //boss.NavMeshAgent.SetDestination(destPos);

            //if(boss.NavMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            //{
            //    Vector2 randomCircle = Random.insideUnitCircle * 10f;
            //    Vector3 randomDest = boss.transform.position
            //        + new Vector3(randomCircle.x, 0f, randomCircle.y);
            //    boss.NavMeshAgent.SetDestination(randomDest);
            //}
        }
    }

}
