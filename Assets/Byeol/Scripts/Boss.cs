using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

        public float Hp { get; set; }

        static public bool bossIsMoved;
        //static public Vector3 meetPlayerPos;

        public bool firstMeetPlayer { get; set; }

        Slider healthSlider;

        public bool attackAnimIsPlaying;

        bool alreadyChecked;
        GameObject dustParticle;
        GameObject clonedDustParticle;

        bool isDead;

        public Boss(Transform boss, Transform player)
        {
            this.transform = boss;

            state = new StrollState();
            state.start(this, player);

            agent = boss.GetComponent<NavMeshAgent>();
            animator = boss.GetComponent<Animator>();

            Hp = 10f;
            firstMeetPlayer = false;

            bossIsMoved = false;

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            healthSlider = canvas.transform.Find("BossBar").GetComponent<Slider>();
            healthSlider.maxValue = Hp;
            healthSlider.value = Hp;

            attackAnimIsPlaying = false;
            alreadyChecked = false;
            dustParticle = Resources.Load<GameObject>("smoke");
            isDead = false;
        }

        public void HandleInput(Transform player)
        {
            if(!isDead)
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

        public void UpdateEnemy(Transform player)
        {
            if(!isDead)
            {
                state.update(this, player);
                checkDuringFighting(player);
            }
        }

        public void TakeDamage(float damage)
        {
            Hp -= damage;
            if (Hp <= 0f)
            {
                if(!isDead)
                {
                    isDead = true;
                    BossControl.bossIsDied = true;

                    Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    Slider healthSlider = canvas.transform.Find("BossBar").GetComponent<Slider>();

                    healthSlider.gameObject.SetActive(false);
                }
            }

            if (healthSlider != null)
            {
                healthSlider.value = Hp;
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
            //attack ¡ﬂ¿œ ∂ß
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
                            player.GetComponent<Player>().setHealth(-4);
                        }
                        alreadyChecked = true;
                        GameObject.Destroy(clonedDustParticle, 1f);
                    }
                }
            }
        }
    }

}