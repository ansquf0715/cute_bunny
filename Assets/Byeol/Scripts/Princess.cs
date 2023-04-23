using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Princess : MonoBehaviour
{
    List<Dictionary<string, object>> princessTalking;
    int talkingNum = 0;

    RaycastHit hit;

    public GameObject npcUI;
    public GameObject backGroundTalking;
    public GameObject npcPortrait;
    public GameObject nameTag;

    Sprite npcPortraitSprite;
    Text talkText;

    string npcName;
    public Sprite princessPortrait;

    bool firstMeet = false;
    bool isStay = false;

    public ParticleSystem particle;

    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        npcName = this.gameObject.name;

        princessTalking = CSVReader.Read("PrincessTalking");
        talkText = backGroundTalking.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStay)
            playerTalkingToNPC();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                isStay = true;
                npcPortrait.GetComponent<Image>().sprite = princessPortrait;
                npcUI.SetActive(true);
                nameTag.SetActive(true);

                if(firstMeet == false) //처음 만나야만 대화 가능
                {
                    firstMeet = true;

                    if(SellingBox.moneyToEscape == false) //충분한 money가 없으면
                    {
                        //talkingNum = 10;
                        //10번부터 돈 가져오라는 얘기
                        for(int i=0; i<3; i++)
                        {
                            Debug.Log("i" + princessTalking[i]["message"]);
                            talkText.text = " " + princessTalking[i]["message"];
                            talkingNum++;
                            break;
                        }
                    }
                    else //충분한 money가 있으면
                    {
                        for (int i = 4; i < princessTalking.Count; i++)
                        {
                            if (princessTalking[i]["name"].ToString() == npcName)
                            {
                                talkText.text = " " + princessTalking[i]["message"];
                                talkingNum++;
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
                else //처음 만난게 아니면 마저 말해야함 -> quest page는 없음
                {
                    if(SellingBox.moneyToEscape == true)
                    {
                        if (princessTalking[talkingNum]["name"].ToString() == npcName)
                        {
                            talkText.text = " " + princessTalking[talkingNum]["message"];
                            talkingNum++;
                        }
                    }
                }
            }
        }
    }

    void playerTalkingToNPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(princessTalking[talkingNum]["name"].ToString()==npcName)
            {
                if(SellingBox.moneyToEscape == false) //돈이 없고
                {
                    if(talkingNum >=3) //말 다 해줬으면 빈칸
                    {
                        talkText.text = " " + princessTalking[13]["message"];
                    }
                    else if(talkingNum<3) //말 다 안해줬으면
                    {
                        talkText.text = " " + princessTalking[talkingNum]["message"];
                        talkingNum++;
                    }
                }
                else
                {
                    talkText.text = " " + princessTalking[talkingNum]["message"];
                    talkingNum++;
                }

                if(talkingNum >= 13)
                {
                    npcUI.SetActive(false);
                    nameTag.SetActive(false);

                    ParticleSystem light = Instantiate(particle,
                        new Vector3(42.59f, 2.85f, 17.47f), Quaternion.identity);

                    StartCoroutine(goToNextScene());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isStay = false;
            npcUI.SetActive(false);
            nameTag.SetActive(false);
        }
    }

    IEnumerator goToNextScene()
    {
        yield return new WaitForSeconds(2f);

        panel.SetActive(true);

        //fade in effect
        CanvasGroup panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        float fadeDuration = 2f;
        float elapsedTime = 0f;
        while(elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, (elapsedTime / fadeDuration));
            panelCanvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panelCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Ending");
    }
}
