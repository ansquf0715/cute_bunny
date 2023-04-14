//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class QuestScript : MonoBehaviour
//{
//    public bool canOpenQuest;

//    public GameObject QuestImage; //quest�� ǥ���� �̹���
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
//        FinishFirstQuest(); //ù��° ����Ʈ Ȯ��
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
//        questName[0] = "���͸� 5���� ��ƿ���!";
//        questName[1] = "����� 5�� �Ⱦ���!";
//        questName[2] = "HP Plus Item�� �� �� �Ⱦ���!";
//        questName[3] = "������ 3�׷� �����!";
//        questName[4] = "�ٽ� ������ ���ƿ���!";
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
//                //Debug.Log("i�� " + i);
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

//    //���� ��� 0 1 2 3 �����ΰ� 0 �����ؼ� 0 1 2 �ƴµ� 3�� �����Ѵٴ���

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
