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

    bool firstMeet;

    // Start is called before the first frame update
    void Start()
    {
        npctalking = CSVReader.Read("npcTalk");

        talkText = backGroundTalking.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getNPCTextImage()
    {
        string npcName = this.gameObject.name;

        if (npcName == "HeartNPC")
        {
            npcPortrait.GetComponent<Image>().sprite = heartPortrait;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //npc가 123번중에 뭔지 결정해서 대화 불러와야 할 듯
        if(other.gameObject.tag == "Player") //player일 경우
        {
            if (Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                getNPCTextImage();
                npcUI.SetActive(true);
                string npcName = this.gameObject.name;
                if(firstMeet == false) //처음 만난거면
                {
                    firstMeet = true;

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
                    for(int i=0; i<npctalking.Count; i++)
                    {
                        if(npctalking[i]["name"].ToString() == npcName)
                        {
                            talkText.text = " " + npctalking[talkingNum]["message"];
                            talkingNum++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Input.GetMouseButtonDown(0))
            {
                string npcName = this.gameObject.name;

                if(npctalking[talkingNum]["name"].ToString() == npcName)
                {
                    talkText.text = " " + npctalking[talkingNum]["message"];
                    talkingNum++;

                    if(talkingNum == 9)
                    {
                        npcUI.SetActive(false);
                    }
                }

            }
        }
    }
}
