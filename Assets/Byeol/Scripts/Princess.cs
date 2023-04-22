using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

                if(firstMeet == true) //처음 만나야만 대화 가능
                {
                    firstMeet = false;

                    for(int i=0; i<princessTalking.Count; i++)
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
                else //처음 만난게 아니면 마저 말해야함 -> quest page는 없음
                {
                    if(princessTalking[talkingNum]["name"].ToString() == npcName)
                    {
                        talkText.text = " " + princessTalking[talkingNum]["message"];
                        talkingNum++;
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
                talkText.text = " " + princessTalking[talkingNum]["message"];
                talkingNum++;

                if(talkingNum >= 10)
                {
                    Debug.Log("ending scene으로 이동");

                    ParticleSystem light = Instantiate(particle,
                        new Vector3(42.59f, 2.85f, 17.47f), Quaternion.identity);
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
}
