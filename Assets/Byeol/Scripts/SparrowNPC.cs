using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SparrowNPC : MonoBehaviour
{
    List<Dictionary<string, object>> npcTalking;
    int talkingNum = 0;

    RaycastHit hit;

    public GameObject npcUI;
    public GameObject backGroundTalking;
    public GameObject npcPortrait;

    Sprite npcPortraitSprite;
    Text talkText;

    string npcName;
    public Sprite birdPortrait;

    bool firstMeet = false;
    bool isStay = false;

    public GameObject hiddenKey; //quest를 위한 key
    GameObject clonedHiddenKey;

    public bool birdQuestIsDone = false;
    bool showBirdQuest = false;

    // Start is called before the first frame update
    void Start()
    {
        npcName = this.gameObject.name;

        npcTalking = CSVReader.Read("BirdTalk");
        talkText = backGroundTalking.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStay)
        {
            playerIsTalkingToNPC();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(FindObjectOfType<HeartNPC>().questIsDone) //heart quest가 끝나야만 대화를 보여줌
        {
            if (other.gameObject.tag == "Player")
            {
                if (Physics.Raycast(other.gameObject.transform.position,
                    other.gameObject.transform.forward, out hit, 1000))
                {
                    isStay = true;
                    npcPortrait.GetComponent<Image>().sprite = birdPortrait;
                    npcUI.SetActive(true);

                    if (firstMeet == false)
                    {
                        firstMeet = true;

                        clonedHiddenKey = Instantiate(hiddenKey, new Vector3(17.73f, 0.3f, 69.27f),
                            Quaternion.identity);

                        for (int i = 0; i < npcTalking.Count; i++)
                        {
                            if (npcTalking[i]["name"].ToString() == npcName)
                            {
                                talkText.text = " " + npcTalking[0]["message"];
                                talkingNum++;
                                break;
                            }
                            else
                                break;
                        }
                    }
                    else //처음 만난게 아니면 quest page를 보여줬는지 아닌지 확인해야함
                    {
                        if (npcTalking[talkingNum]["name"].ToString() == npcName)
                        {
                            if(showBirdQuest) //quest를 보여줬으면
                            {
                                if(!birdQuestIsDone) //quest를 다 안했으면
                                {
                                    talkText.text = " " + npcTalking[6]["message"];
                                }
                                else //quest를 끝냈으면
                                {
                                    talkText.text = " " + npcTalking[7]["message"];
                                    talkingNum++;

                                    if (talkingNum >= 13)
                                        talkText.text = " " + npcTalking[12]["message"];
                                }
                            }

                            talkText.text = " " + npcTalking[talkingNum]["message"];
                            talkingNum++;
                        }
                    }
                }
            }
        }
    }

    void playerIsTalkingToNPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(npcTalking[talkingNum]["name"].ToString() == npcName)
            {
                talkText.text = " " + npcTalking[talkingNum]["message"];
                talkingNum++;

                //몇번째 넘버에서 퀘스트를 불러와야 하는지
                if(talkingNum == 6)
                {
                    npcUI.SetActive(false);
                    FindObjectOfType<QuestManager>().questPageIsOn = true;
                    showBirdQuest = true;
                }

                if(talkingNum >= 13) //고양이한테 가라고 했으면
                {
                    talkText.text = " " + npcTalking[12]["message"];
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
        }
    }
}
