using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StatePattern
{

    public class BossControl : MonoBehaviour
    {
        static public bool toCreateBoss = false;
        static public bool playerIsInFightingZone = false;
        static public bool playerIsMoved = false;
        static public bool bossIsDied = false;

        static public Vector3 firstMetPos;

        Bounds fightingZoneBound;

        public GameObject boss;
        GameObject clonedBoss;

        GameObject player;
        Boss bossObj;

        public GameObject stormParticle;

        bool bossDieChecked = false;

        Image blackImage;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");

            GameObject fightingZone = GameObject.Find("FightingZonePlane");
            Collider fightingZoneCollider = fightingZone.GetComponent<BoxCollider>();
            fightingZoneBound = fightingZoneCollider.bounds;

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            blackImage = canvas.transform.Find("coverBlack").GetComponent<Image>();
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

                //bossObj = new Boss(clonedBoss.transform, player.transform);
                bossObj = Boss.GetInstance(clonedBoss.transform, player.transform);
            }

            if(!bossIsDied)
            {
                if (bossObj != null)
                {
                    bossObj.HandleInput(player.transform);
                    bossObj.UpdateEnemy(player.transform);
                }
            }

            checkPlayerInFightingZone();

            if (bossIsDied)
            {
                if(!bossDieChecked)
                {
                    Debug.Log(" boss is died");
                    bossDieChecked = true;

                    Debug.Log("ee");
                    Animator bossAnim = clonedBoss.GetComponent<Animator>();
                    bossAnim.SetTrigger("Died");
                    Invoke("movePlayerToOriginalPos", 2f);
                }
            }
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

        void movePlayerToOriginalPos()
        {
            //Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            StartCoroutine(ChangeHeightCoroutine());

            
        }

        IEnumerator ChangeHeightCoroutine()
        {
            Debug.Log("change height coroutine");
            blackImage.gameObject.SetActive(true);

            float elapseTime = 0f;
            float changeHeightDuration = 1f;
            float startHeight = blackImage.rectTransform.sizeDelta.y;
            float targetHeight = 1200f;

            while (elapseTime < changeHeightDuration)
            {
                elapseTime += Time.deltaTime;
                float ratio = elapseTime / changeHeightDuration;

                float newHeight = Mathf.Lerp(startHeight, targetHeight, ratio);
                blackImage.rectTransform.sizeDelta = new Vector2(
                    blackImage.rectTransform.sizeDelta.x, newHeight);

                float newAlpha = Mathf.Lerp(0f, 1f, ratio);
                blackImage.color = new Color(blackImage.color.r,
                    blackImage.color.g, blackImage.color.b, newAlpha);

                yield return null;
            }

            GameObject.FindWithTag("Player").transform.position
                = firstMetPos;
            SellingBox.money += 300;
            blackImage.gameObject.SetActive(false);
        }
    }
}