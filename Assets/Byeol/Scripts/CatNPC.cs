using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatNPC : MonoBehaviour
{
    List<Dictionary<string, object>> npcTalking;
    int talkingNum = 0;

    RaycastHit hit;

    public GameObject npcUI;
    public GameObject backGroundTalking;
    public GameObject catPortrait;
    public Sprite catPortraitSprite;

    Text talkText;
    string npcName;

    bool firstMeet = false;
    bool isStay = false;

    public GameObject leftDoor;
    public GameObject rightDoor;

    float moveAmount = 3.0f;
    float moveDuration = 5.0f;

    bool isMoving = false; //door moving
    float moveTimer = 0.0f;
    Vector3 leftDoorTargetPos;
    Vector3 rightDoorTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        npcName = this.gameObject.name;

        npcTalking = CSVReader.Read("CatTalking");
        talkText = backGroundTalking.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStay)
        {
            playerIsTalkingToNPC();
        }

        if(isMoving)
        {
            moveTimer += Time.deltaTime;

            float progress = Mathf.Clamp01(moveTimer / moveDuration);

            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position,
                leftDoorTargetPos, progress);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position,
                rightDoorTargetPos, progress);

            if (progress >= 1.0f)
                isMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(FindObjectOfType<SparrowNPC>().birdQuestIsDone) //bird quest가 끝나야만 대화를 보여줌
        {
            if (other.gameObject.tag == "Player")
            {
                if (Physics.Raycast(other.gameObject.transform.position,
                    other.gameObject.transform.forward, out hit, 1000))
                {
                    isStay = true;
                    catPortrait.GetComponent<Image>().sprite = catPortraitSprite;
                    npcUI.SetActive(true);

                    if (firstMeet == false)
                    {
                        firstMeet = true;

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
                    else
                    {
                        if (npcTalking[talkingNum]["name"].ToString() == npcName)
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

                if(talkingNum == 6)
                {
                    npcUI.SetActive(false);

                    leftDoorTargetPos = leftDoor.transform.position
                        + new Vector3(0.0f, 0.0f, moveAmount);
                    rightDoorTargetPos = rightDoor.transform.position
                        + new Vector3(0.0f, 0.0f, -moveAmount);

                    //start the door movement
                    isMoving = true;
                    moveTimer = 0.0f;
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
