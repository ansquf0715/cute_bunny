using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{

    public class BossControl : MonoBehaviour
    {
        static public bool toCreateBoss = false;
        static public bool playerIsInFightingZone = false;
        static public bool playerIsMoved = false;

        Bounds fightingZoneBound;

        public GameObject boss;
        GameObject clonedBoss;

        GameObject player;
        Boss bossObj;

        public GameObject stormParticle;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");

            GameObject fightingZone = GameObject.Find("FightingZonePlane");
            Collider fightingZoneCollider = fightingZone.GetComponent<BoxCollider>();
            fightingZoneBound = fightingZoneCollider.bounds;
        }

        // Update is called once per frame
        void Update()
        {
            if(clonedBoss == null && toCreateBoss == true)
            {
                toCreateBoss = false;

                clonedBoss =
                    Instantiate(boss, new Vector3(-3.82f, 15f, 56.5f),
                    Quaternion.Euler(0f, 180f, 0));
                Debug.Log("cloned boss" + clonedBoss);

                bossObj = new Boss(clonedBoss.transform, player.transform);
            }

            if(bossObj != null)
            {
                bossObj.HandleInput(player.transform);
                bossObj.UpdateEnemy(player.transform);
            }

            checkPlayerInFightingZone();
        }

        void checkPlayerInFightingZone()
        {
            if (fightingZoneBound.Contains(player.transform.position))
            {
                playerIsInFightingZone = true;
            }
            else
            {
                playerIsInFightingZone = false;
            }
        }

        //void createBoss()
        //{
        //    if (toCreateBoss == true)
        //    {
        //        toCreateBoss = false;

        //        clonedBoss =
        //            Instantiate(boss, new Vector3(-3.82f, 15f, 56.5f),
        //            Quaternion.Euler(0f, 180f, 0));
        //    }
        //}
    }
}