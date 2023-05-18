using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace StatePattern
{
    public abstract class BossState
    {
        virtual public void start(Boss boss, Transform player) { }
        virtual public BossState handleInput(Boss boss, Transform player) { return null; }
        virtual public void update(Boss boss, Transform player) { }
        virtual public void end(Boss boss, Transform player) { }
    }

    public class IdleState:BossState
    {
        public override void start(Boss boss, Transform player)
        {
            Debug.Log("idle state start");
            boss.GetAnimator().SetTrigger("idle");
            boss.Rigidbody.velocity = Vector3.zero;

            Vector3 lookDirection = player.position - boss.transform.position;
            lookDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            boss.transform.rotation = targetRotation;
        }

        public override void update(Boss boss, Transform player)
        {
            Debug.Log("idle state update");
        }
    }

    public class StrollState:BossState
    {
        GameObject bossPlane;
        Collider[] cols = new Collider[3];
        bool playerIsInFightingZone;
        Vector3 randomPos;

        public override void start(Boss boss, Transform player)
        {
            //Debug.Log("stroll state start");

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
                //Debug.Log("stroll update");
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

            boss.NavMeshAgent.speed = 7f;

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
            //Debug.Log("run state start");
            destPos = boss.transform.position;
            lastPlayerPos = player.position;
            startTime = Time.time;
            stormParticle = Resources.Load<GameObject>("BossParticle");

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            blackImage = canvas.transform.Find("coverBlack").GetComponent<Image>();
            HealthSlider = canvas.transform.Find("BossBar").GetComponent<Slider>();

            startHeight = blackImage.rectTransform.sizeDelta.y;

            boss.GetAnimator().SetBool("bossRun", true);
            boss.NavMeshAgent.speed = 30f;
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(player.position, boss.transform.position);

            if(Boss.bossIsMoved)
            {
                if (distance < 10f)
                {
                    if (boss.Hp <= (boss.maxHp / 2))
                    {
                        //Debug.Log("boss max hp / 2");
                        return new BulletAttackState();
                    }
                    else
                    {
                        return new AttackState();
                    }
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

                GameObject backAudio = GameObject.Find("AudioManager");
                AudioSource backSource = backAudio.GetComponent<AudioSource>();
                backSource.volume = 0f;

                GameObject audioObject = GameObject.Find("BossSound");
                AudioSource audioSource = audioObject.GetComponent<AudioSource>();
                audioSource.Play();
                
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

        //GameObject dustParticle;
        //GameObject clonedDustParticle;

        public override void start(Boss boss, Transform player)
        {
            //Debug.Log("attack state start");
            changeState = false;
            isAttacking = false;

            randomMotionName = getRandomMotionName();

            hasCollided = false;
            isDustParticleCreated = false;

            //dustParticle = Resources.Load<GameObject>("smoke");
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(player.position, boss.transform.position);

            if(boss.Hp >= 3f)
            {
                if (distance >= 3f)
                {
                    boss.GetAnimator().SetBool(randomMotionName, false);
                    boss.attackAnimIsPlaying = false;

                    return new RunState();
                }

                if (changeState)
                {
                    boss.GetAnimator().SetBool(randomMotionName, false);
                    return new RunState();
                }
            }
            else if(boss.Hp <3f)
            {
                boss.GetAnimator().SetBool(randomMotionName, false);
                boss.GetAnimator().SetBool("flee", true);
                return new FleeState();
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
                boss.attackAnimIsPlaying = true;

                AudioClip punchSound = Resources.Load<AudioClip>("bosspunch");
                boss.AudioSource.clip = punchSound;
                boss.AudioSource.Play();
            }
            else
            {
                if (!IsPlayingAnimation(boss))
                {
                    //GameObject.Destroy(clonedDustParticle, 1f);
                    isAttacking = false;
                    changeState = true;
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

    public class BulletAttackState:BossState
    {
        float distance;
        bool changeState;

        GameObject pineapple;
        GameObject clonedPineapple;
        float throwForce;
        float pineappleRadius;

        bool alreadyAttacked;
        float attackTimer;

        bool isThrowing;

        public override void start(Boss boss, Transform player)
        {
            //Debug.Log("bullet attack state start");
            pineapple = Resources.Load<GameObject>("Pineapple");
            throwForce = 7f;
            alreadyAttacked = false;
            attackTimer = 0f;
            changeState = false;
            pineappleRadius = 0.5f;
            isThrowing = false;
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            distance = Vector3.Distance(player.position, boss.transform.position);

            if(boss.Hp>=3f)
            {
                if(distance >= 3f && attackTimer >= 3f)
                {
                    //boss.GetAnimator().SetBool("throw", false);
                    return new RunState();
                }
            }
            else if(boss.Hp < 3f)
            {
                //boss.GetAnimator().SetBool("throw", false);
                return new FleeState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            if(!alreadyAttacked)
            {
                throwPineapple(boss, player);
                alreadyAttacked = true;
            }
            if(isThrowing && boss.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                boss.GetAnimator().SetBool("throw", false);
                isThrowing = false;
            }

            attackTimer += Time.deltaTime;
        }

        void throwPineapple(Boss boss, Transform player)
        {
            //Debug.Log("throw pineapple");

            //boss.GetAnimator().SetBool("throw", true);
            boss.GetAnimator().SetTrigger("Throw");
            isThrowing = true;

            Vector3 newPos = boss.transform.position;
            newPos += boss.transform.right * 3f;
            newPos.y += 4f;

            clonedPineapple = GameObject.Instantiate(
                pineapple, newPos, Quaternion.identity);
            Rigidbody pineappleRB = clonedPineapple.AddComponent<Rigidbody>();
            AudioClip pineappleSound = Resources.Load<AudioClip>("bosspineapple");
            boss.AudioSource.clip = pineappleSound;
            boss.AudioSource.Play();

            //Vector3 direction = (player.position - boss.transform.position).normalized;
            Vector3 direction = boss.transform.forward;
            Vector3 throwVelocity = direction * 10f;

            pineappleRB.velocity = throwVelocity;

            //pineappleRB.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
            pineappleRB.AddForce(Vector3.up * 3f, ForceMode.Impulse);
            pineappleRB.AddForce(direction * throwForce, ForceMode.Impulse);
            //pineappleRB.AddForce(direction, ForceMode.Impulse);
        }
    }

    public class FleeState : BossState
    {
        float step;
        Vector3 directionToPlayer;

        float hpIncreaseRate = 0.5f; //1초당 Hp 증가량
        float hpIncreaseTimer = 1f; //1초당 Hp 증가 타이머

        public override void start(Boss boss, Transform player)
        {
            //Debug.Log("Flee state start");
            boss.GetAnimator().SetBool("bossWalking", true);

            //boss가 player의 반대 방향을 바라보도록 설정
            Vector3 bossPos = boss.transform.position;
            Vector3 playerPos = player.position;

            directionToPlayer = playerPos - bossPos;
            directionToPlayer.y = 0f;

            Quaternion targetRot = Quaternion.LookRotation(-directionToPlayer);
            boss.transform.rotation = targetRot;
        }

        public override BossState handleInput(Boss boss, Transform player)
        {
            if (boss.Hp >= 4)
            {
                return new WalkState();
            }
            return null;
        }

        public override void update(Boss boss, Transform player)
        {
            hpIncreaseTimer -= Time.deltaTime;

            if(hpIncreaseTimer <= 0f)
            {
                boss.IncreaseHP(hpIncreaseRate);
                hpIncreaseTimer = 1f;
            }

            boss.GetAnimator().SetBool("flee", false);

            Vector3 fleeDirection = (boss.transform.position - player.position).normalized;
            Vector3 fleePos = boss.transform.position + fleeDirection;
            boss.NavMeshAgent.SetDestination(fleePos);
            boss.NavMeshAgent.speed = 10f;
            //Debug.Log("fleePos" + fleePos);
        }
    }

}
