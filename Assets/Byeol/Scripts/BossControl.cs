using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

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

        public GameObject victory;
        public GameObject victoryParticle;

        bool reachedOneThirdHP;
        bool showFirstLight;
        bool reachedTwoThirdHP;
        bool showSecondLight;
        GameObject lightEffect;
        GameObject clonedLightEffect;

        static public int diedBabyBoss;

        bool blackImageActive = false;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");

            GameObject fightingZone = GameObject.Find("FightingZonePlane");
            Collider fightingZoneCollider = fightingZone.GetComponent<BoxCollider>();
            fightingZoneBound = fightingZoneCollider.bounds;

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            blackImage = canvas.transform.Find("coverBlack").GetComponent<Image>();

            reachedOneThirdHP = false;
            showFirstLight = false;
            reachedTwoThirdHP = false;
            showSecondLight = false;
            lightEffect = Resources.Load<GameObject>("BossLight");
            clonedLightEffect = null;
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

                    Animator bossAnim = clonedBoss.GetComponent<Animator>();
                    bossAnim.SetTrigger("Died");

                    Invoke("createBabyBoss", 1f);
                }
            }

            if(diedBabyBoss >= 2)
            {
                diedBabyBoss = 0;
                Invoke("movePlayerToOriginalPos", 2f);
            }
        }

        void createBabyBoss()
        {
            GameObject babyBoss1 = Resources.Load<GameObject>("babyBoss1");
            GameObject babyBoss2 = Resources.Load<GameObject>("babyBoss2");

            Vector3 babyBoss1Pos = clonedBoss.transform.position;
            babyBoss1Pos.x += 5;

            Vector3 babyBoss2Pos = clonedBoss.transform.position;
            babyBoss2Pos.x -= 5;

            GameObject clonedBabyBoss1 = Instantiate(
                babyBoss1, babyBoss1Pos, Quaternion.identity);
            clonedBabyBoss1.GetComponent<NavMeshAgent>().speed = 10f;

            GameObject clonedBabyBoss2 = Instantiate(
                babyBoss2, babyBoss2Pos, Quaternion.identity);
            clonedBabyBoss2.GetComponent<NavMeshAgent>().speed = 7f;
        }

        void createLight()
        {
            float maxHP = 20f;
            float oneThirdHp = maxHP / 3f;
            float twoThirdHp = (maxHP / 3f) * 2f;

            if(bossObj.Hp <= oneThirdHp && !reachedOneThirdHP)
            {
                reachedOneThirdHP = true;
                GenerateLightEffect();
            }
            else if(bossObj.Hp <= twoThirdHp && !reachedTwoThirdHP)
            {
                reachedTwoThirdHP = true;
                GenerateLightEffect();
            }

            if(clonedLightEffect != null)
            {
                clonedLightEffect.transform.position -= new Vector3(0f, 3f, 0f) * Time.deltaTime;
                Debug.Log("light effect position" + clonedLightEffect.transform.position);
                if(clonedLightEffect.transform.position.y <= 0f)
                {
                    Destroy(clonedLightEffect);
                    clonedLightEffect = null;
                }
            }
        }

        void GenerateLightEffect()
        {
            if(clonedLightEffect == null)
            {
                Vector3 newPos = Player.playerPos;
                newPos.y += 20;

                clonedLightEffect = Instantiate(
                    lightEffect, newPos, Quaternion.identity);
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
            if(blackImageActive)
            {
                GameObject backAudio = GameObject.Find("AudioManager");
                AudioSource backSource = backAudio.GetComponent<AudioSource>();
                backSource.volume = 1f;
            }
        }

        IEnumerator ChangeHeightCoroutine()
        {
            Debug.Log("change height coroutine");

            GameObject audioObject = GameObject.Find("BossSound");
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            audioSource.Stop();

            Vector3 newPos = player.transform.position;
            newPos.y += 3;

            GameObject panpare = Instantiate(victoryParticle, newPos, Quaternion.identity);
            yield return new WaitForSeconds(0.8f);

            RectTransform canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
            Vector2 canvasCenter = new Vector2(
                canvasTransform.rect.width * 0.5f, canvasTransform.rect.height * 0.5f);

            GameObject vic = Instantiate(victory, canvasCenter, Quaternion.identity,
                GameObject.Find("Canvas").transform);

            Destroy(vic, 1f);
            Destroy(panpare, 1f);

            yield return new WaitForSeconds(1f);

            Destroy(clonedBoss, 1f);

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
            blackImageActive = true;
        }
    }
}