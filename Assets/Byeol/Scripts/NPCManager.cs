using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCManager : MonoBehaviour
{
    RaycastHit hit; //raycast에 닿였을 때 hit에 저장

    public TalkManager talkManager;
    //public QuestManager questManager;

    public Text talkText; //대화창에 텍스트 띄워줌

    public GameObject TalkImage; //대화창 배경

    public GameObject scanObject; //raycast로 확인한 오브젝트를 저장
    public Image portraitImage; //오브젝트의 이미지

    //public Text questText;

    //isMove를 콜리전 여부에 따라 변경할 것
    public bool isMove; //플레이어가 움직일 수 있는지 없는지
    public int talkIndex = 0; //몇번째 대화가 이어지는지

    // Start is called before the first frame update
    void Start()
    {
        talkManager = FindObjectOfType<TalkManager>();
        //questManager = FindObjectOfType<QuestManager>();

        //questText.text = questManager.CheckQuest();

        //talkText.gameObject.SetActive(false);
        TalkImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText(GameObject scanObj)
    {
        //Debug.Log("Show Text 불려짐");
        scanObject = scanObj;
        ObjectData objectData = scanObject.GetComponent<ObjectData>();
        OnTalk(objectData.id, objectData.isNpc);

        TalkImage.SetActive(true);
    }



    void OnTalk(int id, bool isNpc)
    {
        //Debug.Log("id" + id);
        //Debug.Log("isNpc" + isNpc);

        //int questTalkIndex = questManager.GetQuestTalkIndex(id);

        string talkData = talkManager.GetTalk(id , talkIndex);
        //talkIndex++;
        if(talkIndex == 7) //마지막 말이 끝나면
        {
            FindObjectOfType<QuestScript>().setOpenQuest(); //quest를 open할 수 있게
            bool finishedQuest = FindObjectOfType<QuestScript>().FinishedQuests;
            //Debug.Log("finished QUest" + finishedQuest);
            if (finishedQuest == true)
            {
                talkIndex++;
            }
        }

        if(talkIndex == 8 || talkIndex == 9)
        {
            talkIndex++;
        }

        if(talkIndex == 10)
        {
            //SceneManager.LoadScene("Ending");
            Invoke("load", 2);
        }
        //if(talkIndex == 8) //이건지 10인지 확인할 것
        ////if(talkIndex == 10) //마지막 말
        //{
        //    FindObjectOfType<QuestScript>().FinishFifthQuest();
        //    talkIndex++;
        //}

        //if (talkIndex == 9 || talkIndex == 10)
        //    talkIndex++;

        //if(talkIndex == 11)
        //{
        //    SceneManager.LoadScene("Ending");
        //}

        //Debug.Log("talkIndex" + talkIndex);

        if (talkData == null)
        {
            isMove = false;
            talkIndex = 0;
            //questText.text = questManager.CheckQuest(id);
            talkIndex++;

            return;
        }

        if(isNpc)
        {
            TalkImage.SetActive(true);
            talkText.text = talkData;
            portraitImage.sprite = talkManager.GetSprite(id);
            portraitImage.color = new Color(1, 1, 1, 1);
            talkIndex++;

        }

        else
        {

            talkText.text = talkData;
            Color color = portraitImage.color;
            color.a = 0f;
            portraitImage.color = color;
            //portraitImage.color = new Color(1, 1, 1, 0);
            talkIndex++;

        }

        //isMove = true;
        //talkIndex++;
        //Debug.Log("talkIndex" + talkIndex);
    }

    public void checkImageSetting()
    {
        TalkImage.SetActive(false);
    }

    void load()
    {
        SceneManager.LoadScene("Ending");
    }

}
