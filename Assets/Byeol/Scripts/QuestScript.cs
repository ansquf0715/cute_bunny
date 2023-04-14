//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class QuestScript : MonoBehaviour
//{
//    public bool canOpenQuest;

//    public GameObject QuestImage; //quest를 표시할 이미지
//    public bool QuestActivated = false;

//    //List<Text> questTexts = new List<Text>();
//    //List<string> questName = new List<string>();

//    Text[] questTexts = new Text[5];
//    string[] questName = new string[5];

//    public int enemyDiedCount = 0;
//    public bool FinishedQuests = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        QuestImage.SetActive(false);
//        saveQuestName();
//        saveQuestText();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        TryOpenQuest();
//        FinishFirstQuest(); //첫번째 퀘스트 확인
//    }

//    private void TryOpenQuest()
//    {
//        if (canOpenQuest == true)
//        {
//            if (Input.GetKeyDown(KeyCode.Q))
//            {
//                QuestActivated = !QuestActivated;

//                if (QuestActivated)
//                {
//                    OpenQuest();
//                    showQuest();
//                }
//                else
//                    CloseQuest();
//            }
//        }

//    }

//    private void OpenQuest()
//    {
//        QuestImage.SetActive(true);
//    }

//    private void CloseQuest()
//    {
//        QuestImage.SetActive(false);
//    }

//    private void saveQuestName()
//    {
//        questName[0] = "몬스터를 5마리 잡아와줘!";
//        questName[1] = "사과를 5개 팔아줘!";
//        questName[2] = "HP Plus Item을 한 개 팔아줘!";
//        questName[3] = "나무를 3그루 베어봐!";
//        questName[4] = "다시 나한테 돌아와줘!";
//    }

//    private void saveQuestText()
//    {
//        questTexts[0] = QuestImage.transform.GetChild(0).GetComponent<Text>();
//        questTexts[1] = QuestImage.transform.GetChild(1).GetComponent<Text>();
//        questTexts[2] = QuestImage.transform.GetChild(2).GetComponent<Text>();
//        questTexts[3] = QuestImage.transform.GetChild(3).GetComponent<Text>();
//        questTexts[4] = QuestImage.transform.GetChild(4).GetComponent<Text>();
//    }

//    private void showQuest()
//    {
//        for(int i=0; i<questTexts.Length; i++)
//        {
//            if(questTexts[i] != null)
//            {
//                //Debug.Log("i값 " + i);
//                questTexts[i].text = questName[i];
//            }
//        }
//    }

//    public void setOpenQuest()
//    {
//        canOpenQuest = true;
//    }

//    public void checkFirstQuest()
//    {
//        enemyDiedCount++;
//    }

//    //예를 들어 0 1 2 3 만들어두고 0 삭제해서 0 1 2 됐는데 3에 접근한다던가

//    void FinishFirstQuest()
//    {
//        if(enemyDiedCount == 1)
//        {
//            Destroy(questTexts[0]);

//            questTexts[0] = null;
//            questName[0] = " ";
//        }
//    }

//    public void FinishSecondQuest()
//    {
//        Destroy(questTexts[1]);

//        questTexts[1] = null;
//        questName[1] = " ";
//    }

//    public void FinishThirdQuest()
//    {
//        Destroy(questTexts[2]);

//        questTexts[2] = null;
//        questName[2] = " ";
//    }

//    public void FinishFourthQuest()
//    {
//        Destroy(questTexts[3]);

//        questTexts[3] = null;
//        questName[3] = " ";
//    }

//    public void FinishFifthQuest()
//    {
//        Destroy(questTexts[4]);

//        questTexts[4] = null;
//        questName[4] = " ";
//    }
//}
