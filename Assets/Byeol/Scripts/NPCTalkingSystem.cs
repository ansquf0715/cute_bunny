using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalkingSystem : MonoBehaviour
{
    List<Dictionary<string, object>> npctalking;
    int talkingNum = 0;

    RaycastHit hit; //ray에 닿았을 때 hit에 저장(?)

    public GameObject npcUI;
    public GameObject backGroundTalking;

    public GameObject npcPortrait;
    Sprite npcPortraitSprite;
    Text talkText;

    public Sprite heartPortrait;
    public Sprite birdPortrait;

    bool firstMeet = false;
    bool isStay = false;

    // Start is called before the first frame update
    void Start()
    {
        npctalking = CSVReader.Read("npcTalk");

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

    void getNPCTextImage()
    {
        string npcName = this.gameObject.name;

        if (npcName == "HeartNPC")
        {
            npcPortrait.GetComponent<Image>().sprite = heartPortrait;
        }
        else if(npcName == "Sparrow")
        {
            npcPortrait.GetComponent<Image>().sprite = birdPortrait;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") //player일 경우
        {
            if (Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                
                isStay = true;

                getNPCTextImage();
                npcUI.SetActive(true);
                string npcName = this.gameObject.name;

                Debug.Log("npcName" + npcName);
                if(firstMeet == false) //처음 만난거면
                {
                    firstMeet = true;

                    Debug.Log("npctalking count " + npctalking.Count);
                    for(int i=0; i<npctalking.Count; i++)
                    {
                        if(npctalking[i]["name"].ToString() == npcName)
                        {
                            talkText.text = " " + npctalking[0]["message"];
                            talkingNum++;

                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                } 
                else
                {
                    if(npctalking[talkingNum]["name"].ToString() == npcName)
                    {
                        talkText.text = " " + npctalking[talkingNum]["message"];
                        talkingNum++;
                    }
                }
            }

        }
    }

    void playerIsTalkingToNPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse input");

            string npcName = this.gameObject.name;

            if(npctalking[talkingNum]["name"].ToString() == npcName)
            {
                talkText.text = " " + npctalking[talkingNum]["message"];
                talkingNum++;

                //if(talkingNum == 11)
                //{
                //    npcUI.SetActive(false);
                //    FindObjectOfType<QuestManager>().questPageIsOn = true;
                //}
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