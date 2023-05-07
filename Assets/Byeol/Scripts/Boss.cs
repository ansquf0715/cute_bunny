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
        static public Vector3 meetPlayerPos;

        public bool firstMeetPlayer { get; set; }

        Slider healthSlider;

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
            //healthSlider.value = 1f;
            healthSlider.maxValue = Hp;
            healthSlider.value = Hp;
        }

        public void HandleInput(Transform player)
        {
            BossState state = this.state.handleInput(this, player);
            if(state != null)
            {
                this.state.end(this, player);
                this.state = state;
                this.state.start(this, player);
            }
        }

        public void UpdateEnemy(Transform player)
        {
            state.update(this, player);
            //Debug.Log("boss hp" + Hp);
        }

        public void TakeDamage(float damage)
        {
            Hp -= damage;
            //if(Hp <= 0f)
            //{
            //    Debug.Log("Die");
            //}

            if(healthSlider != null)
            {
                healthSlider.value = Hp;
            }
        }

        public BossState GetCurrentState()
        {
            return state;
        }

    }

}