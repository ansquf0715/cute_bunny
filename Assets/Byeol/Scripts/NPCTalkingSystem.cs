using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalkingSystem : MonoBehaviour
{
    List<Dictionary<string, object>> npctalking;
    int talkingNum = 0;

    RaycastHit hit; //ray�� ����� �� hit�� ����(?)

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
        //npc�� 123���߿� ���� �����ؼ� ��ȭ �ҷ��;� �� ��
        if(other.gameObject.tag == "Player") //player�� ���
        {
            if (Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                getNPCTextImage();
                npcUI.SetActive(true);
                string npcName = this.gameObject.name;
                if(firstMeet == false) //ó�� �����Ÿ�
                {
                    firstMeet = true;

                    Debug.Log("npctalking count " + npctalking.Count);
                    for(int i=0; i<npctalking.Count; i++)
                    {
                        if(npctalking[i]["name"].ToString() == npcName)
                        {
                            talkText.text = " " + npctalking[0]["message"];
                            talkingNum++;

                            //Debug.Log("talking Num" + talkingNum);

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

                    //Debug.Log("talking Num" + talkingNum);

                    if (talkingNum == 10)
                    {
                        npcUI.SetActive(false);
                        //Debug.Log("��ȭ ��");
                        FindObjectOfType<QuestManager>().questPageIsOn = true;
                    }
                }
            }
            //if(talkingNum == 10)
            //{
            //    npcUI.SetActive(false);
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("talking Num" + talkingNum);

            npcUI.SetActive(false);
        }
    }
}
