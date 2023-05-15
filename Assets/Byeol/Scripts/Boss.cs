using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

namespace StatePattern
{
    public class Boss 
    {
        private static Boss instance;
        public static Boss GetInstance(Transform bossTransform = null,
            Transform playerTransform = null)
        {
            if (instance == null)
            {
                instance = new Boss(bossTransform, playerTransform);
            }

            if(bossTransform == null && playerTransform == null)
            {
                return instance;
            }
            return instance;
        }

        public Transform transform;
        BossState state;

        public NavMeshAgent NavMeshAgent { get { return agent; } }
        NavMeshAgent agent;

        Animator animator;
        public Animator GetAnimator()
        {
            if (animator == null)
            {
                animator = transform.GetComponent<Animator>();
            }
            return animator;
        }

        public Rigidbody Rigidbody { get { return rigidbody; } }
        Rigidbody rigidbody;

        public float Hp { get; set; }
        public float maxHp;

        float bossDamage;

        static public bool bossIsMoved;
        //static public Vector3 meetPlayerPos;

        public bool firstMeetPlayer { get; set; }

        Slider healthSlider;
        TMP_Text playerDamage;

        public bool attackAnimIsPlaying;

        bool alreadyChecked;
        GameObject dustParticle;
        GameObject clonedDustParticle;

        bool reachedOneThirdHP;
        bool reachedTwoThirdHP;
        GameObject lightEffect;
        public GameObject clonedLightEffect;

        bool isDead;

        public Boss(Transform boss, Transform player)
        {
            this.transform = boss;

            state = new StrollState();
            state.start(this, player);

            agent = boss.GetComponent<NavMeshAgent>();
            animator = boss.GetComponent<Animator>();
            rigidbody = boss.GetComponent<Rigidbody>();

            maxHp = 20f;
            //Hp = 20f;
            Hp = maxHp;
            firstMeetPlayer = false;

            bossDamage = -2f;
            bossIsMoved = false;

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            healthSlider = canvas.transform.Find("BossBar").GetComponent<Slider>();
            healthSlider.maxValue = Hp;
            healthSlider.value = Hp;

            playerDamage = canvas.transform.Find("Dam").GetComponent<TMP_Text>();

            attackAnimIsPlaying = false;
            alreadyChecked = false;
            dustParticle = Resources.Load<GameObject>("smoke");

            reachedOneThirdHP = false;
            reachedTwoThirdHP = false;
            lightEffect = Resources.Load<GameObject>("BossLight");
            clonedLightEffect = null;

            isDead = false;
        }

        public void HandleInput(Transform player)
        {
            if(!isDead)
            {
                if (player.GetComponent<Player>().getHealth() <= 0)
                {
                    this.state.end(this, player);
                    this.state = new IdleState();
                    this.state.start(this, player);
                }
                else
                {
                    BossState state = this.state.handleInput(this, player);
                    if (state != null)
                    {
                        this.state.end(this, player);
                        this.state = state;
                        this.state.start(this, player);
                    }
                }
            }
        }

        public void UpdateEnemy(Transform player)
        {
            if(!isDead)
            {
                if(player.GetComponent<Player>().getHealth() <= 0)
                {
                    state.update(this, player);
                }
                else
                {
                    state.update(this, player);
                    checkDuringFighting(player);
                }
            }

            if (clonedLightEffect != null)
            {
                clonedLightEffect.transform.position -= new Vector3(0f, 6f, 0f) * Time.deltaTime;
                if (clonedLightEffect.transform.position.y <= 0f)
                {
                    GameObject.Destroy(clonedLightEffect);
                    clonedLightEffect = null;
                }
                else
                {
                    Vector3 playerPositoin = Player.playerPos;
                    float distance = Vector3.Distance(playerPositoin, clonedLightEffect.transform.position);
                    if(distance <= 3f)
                    {
                        GameObject.Destroy(clonedLightEffect);
                        clonedLightEffect = null;
                        player.GetComponent<Player>().setHealth(-10f);
                    }
                }
            }
        }

        public void TakeDamage(float damage)
        {
            animator.SetTrigger("hit");

            Vector3 newPos = this.transform.position;
            newPos.y += 10f;
            Vector3 relativePosition = Camera.main.WorldToViewportPoint(newPos);
            Vector3 canvasPosition = new Vector3(
                relativePosition.x * Camera.main.pixelWidth,
                relativePosition.y * Camera.main.pixelHeight, 0f);

            //playerDamage.rectTransform.position = canvasPosition;

            GameObject textPrefab = Resources.Load<GameObject>("Dam");
            GameObject damageText = GameObject.Instantiate(textPrefab, canvasPosition, Quaternion.identity,
                GameObject.Find("Canvas").transform);
            TMP_Text text = damageText.GetComponent<TMP_Text>();
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            text.color = randomColor;
            text.text = 
                (-1 * GameObject.FindWithTag("Player").GetComponent<Player>().getPower()).ToString();
            GameObject.Destroy(damageText, 1f);
            text.outlineColor = Color.white;
            text.outlineWidth = 0.1f;

            Hp -= damage;
            if (Hp <= 0f)
            {
                if(!isDead)
                {
                    isDead = true;
                    BossControl.bossIsDied = true;
                    NavMeshAgent.isStopped = true;
                    bossIsMoved = false;

                    if(clonedLightEffect != null)
                    {
                        GameObject.Destroy(clonedLightEffect);
                    }

                    Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    Slider healthSlider = canvas.transform.Find("BossBar").GetComponent<Slider>();

                    healthSlider.gameObject.SetActive(false);
                }
            }
            else
            {
                float maxHP = 20f;
                float oneThirdHp = maxHP / 3f;
                float twoThirdHp = (maxHP / 3f) * 2f;

                if (Hp <= oneThirdHp && !reachedOneThirdHP)
                {
                    reachedOneThirdHP = true;
                    GenerateLightEffect();
                    bossDamage = -4f;
                }
                else if (Hp <= twoThirdHp && !reachedTwoThirdHP)
                {
                    reachedTwoThirdHP = true;
                    GenerateLightEffect();
                    bossDamage = -6f;
                }
            }

            if (healthSlider != null)
            {
                healthSlider.value = Hp;
            }
        }

        void GenerateLightEffect()
        {
            Debug.Log("generate light effect");
            if (clonedLightEffect == null)
            {
                Vector3 newPos = Player.playerPos;
                newPos.y += 20;

                clonedLightEffect = GameObject.Instantiate(
                    lightEffect, newPos, Quaternion.identity);
            }
        }

        public void IncreaseHP(float changedHP)
        {
            Hp += changedHP;
            healthSlider.value = Hp;
        }

        public BossState GetCurrentState()
        {
            return state;
        }

        public void checkDuringFighting(Transform player)
        {
            //attack ���� ��
            if(attackAnimIsPlaying)
            {
                MeshCollider meshCollider =
                    transform.GetComponentInChildren<MeshCollider>();
                //Debug.Log("mesh collider" + meshCollider);
                if(player.GetComponent<Collider>().bounds.Intersects(meshCollider.bounds))
                {
                    Debug.Log("player and boss fight");
                    alreadyChecked = false;
                    if (!alreadyChecked)
                    {
                        Debug.Log("dust particle instantiate");
                        if(clonedDustParticle == null)
                        {
                            Vector3 newPos = this.transform.position;
                            newPos.x = this.transform.position.x + 3;
                            newPos.z = this.transform.position.z + 3;
                            clonedDustParticle = GameObject.Instantiate(
                                dustParticle, newPos, Quaternion.identity);

                            ParticleSystem dustSystem = clonedDustParticle.GetComponent<ParticleSystem>();
                            if (!dustSystem.isPlaying)
                                dustSystem.Play();
                            player.GetComponent<Player>().setHealth(bossDamage);
                            //player died
                            if(player.GetComponent<Player>().getHealth() <=0)
                            {
                                BossControl.firstMetPos = new Vector3(0, 0, 0);
                                healthSlider.gameObject.SetActive(false);
                            }
                        }
                        alreadyChecked = true;
                        GameObject.Destroy(clonedDustParticle, 1f);
                    }
                }
            }
        }
    }

}