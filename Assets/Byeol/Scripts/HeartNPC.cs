using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartNPC : MonoBehaviour
{
    List<Dictionary<string, object>> npcTalking;
    int talkingNum = 0;

    RaycastHit hit;

    public GameObject npcUI;
    public GameObject backGroundTalking;
    public GameObject npcPortrait;

    //Sprite npcPortraitSprite;
    Text talkText;

    string npcName;
    public Sprite heartPortrait;

    bool firstMeet = true;
    bool isStay = false;
    bool showQuest = false;

    public bool questIsDone = false;

    public static bool heartCommunicationOver = false;

    // Start is called before the first frame update
    void Start()
    {
        npcName = this.gameObject.name;

        npcTalking = CSVReader.Read("heartTalk");
        talkText = backGroundTalking.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStay)
        {
            playerIsTalkingToNPC();
        }

        //if (talkingNum >= 12)
        //    heartCommunicationOver = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                isStay = true;

                npcPortrait.GetComponent<Image>().sprite = heartPortrait;
                npcUI.SetActive(true);

                if(firstMeet == true)
                {
                    firstMeet = false;

                    for(int i=0; i<npcTalking.Count; i++)
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
                    if(npcTalking[talkingNum]["name"].ToString() == npcName)
                    {
                        if(showQuest) //quest를 보여줬으면
                        {
                            if(!questIsDone) //quest를 다 안했으면
                            {
                                talkText.text = " " + npcTalking[11]["message"];
                            }
                            else //quest를 끝냈으면
                            {
                                talkText.text = " " + npcTalking[12]["message"];
                                talkingNum++;

                                if (talkingNum >= 15)
                                    talkText.text = " " + npcTalking[14]["message"];
                            }
                        }
                        else //quest를 안보여줬으면
                        {
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

                if(talkingNum == 11)
                {
                    npcUI.SetActive(false);
                    //FindObjectOfType<QuestManager>().questPageIsOn = true;
                    showQuest = true;
                }



                if (talkingNum >= 15) //새한테 가라고 했으면
                {
                    talkText.text = " " + npcTalking[14]["message"];
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
