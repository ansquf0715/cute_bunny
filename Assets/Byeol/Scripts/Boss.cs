using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace StatePattern
{
    public class Boss 
    {
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

        public bool bossIsMoved;
        static public Vector3 meetPlayerPos;

        public bool firstMeetPlayer { get; set; }

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
        }

    }

}