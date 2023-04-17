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

    bool firstMeet = false;
    bool isStay = false;

    // Start is called before the first frame update
    void Start()
    {
        npcName = this.gameObject.name;

        npcTalking = CSVReader.Read("heartTalk");
        talkText = backGroundTalking.GetComponentInChildren<Text>();

        Debug.Log("talk text " + talkText);
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
        if(other.gameObject.tag == "Player")
        {
            if(Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                isStay = true;

                //getNPCTextImage();
                npcPortrait.GetComponent<Image>().sprite = heartPortrait;
                npcUI.SetActive(true);

                if(firstMeet == false)
                {
                    firstMeet = true;

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
                else
                {
                    if(npcTalking[talkingNum]["name"].ToString() == npcName)
                    {
                        talkText.text = " " + npcTalking[talkingNum]["message"];
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
            if(npcTalking[talkingNum]["name"].ToString() == npcName)
            {
                talkText.text = " " + npcTalking[talkingNum]["message"];
                talkingNum++;

                if(talkingNum == 11)
                {
                    npcUI.SetActive(false);
                    FindObjectOfType<QuestManager>().questPageIsOn = true;
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
