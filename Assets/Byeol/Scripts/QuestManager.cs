using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    List<Dictionary<string, object>> questCSV;

    public GameObject QuestImage;

    //List<Text> questTexts = new List<Text>(); //quest 출력
    List<TextMeshProUGUI> questTexts = new List<TextMeshProUGUI>();
    //List<string> questContents = new List<string>(); //quest 내용

    public bool questPageIsOn = false;

    bool changeToBirdQuest = false;

    bool[] checkHeartQ = new bool[5]; //heartQuest 됐는지 확인
    bool[] checkBirdQ = new bool[4]; //birdQuest했는지 확인

    //to check heart quest
    public static int diedEnemyCount;
    public static int sellAppleCount;
    public static int sellHPPlusItemCount;
    public static int deathTreesCount;
    public static int plantedTreesCount;

    //to check bird quest
    public static int plantedBirdTreesCount;

    // Start is called before the first frame update
    void Start()
    {
        questCSV = CSVReader.Read("Quest");
        QuestImage.SetActive(false);

        //Text[] texts = QuestImage.GetComponentsInChildren<Text>();
        TextMeshProUGUI[] texts = QuestImage.GetComponentsInChildren<TextMeshProUGUI>();
        for(int i=0; i<texts.Length; i++)
        {
            questTexts.Add(texts[i]);
        }

        diedEnemyCount = 0;
        for (int i = 0; i < checkHeartQ.Length; i++)
            //checkHeartQ[i] = false;
            checkHeartQ[i] = true;

        showContents();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        showQuestPage();

        if(!checkAllHeartQuestIsDone())
        {
            checkHeartQuestFinished();
        }
        if (checkAllHeartQuestIsDone()) //heart quest를 다 했으면
        {
            FindObjectOfType<HeartNPC>().questIsDone = true;

            if(changeToBirdQuest == false) //quest내용 바꾸기
            {
                changeContents();
                changeToBirdQuest = true;
            }

            checkBirdQuestFinished();
        }
    }

    bool checkAllHeartQuestIsDone()
    {
        for(int i=0; i<checkHeartQ.Length; i++)
        {
            if (checkHeartQ[i] == false)
                return false;
        }
        return true;
    }

    void showContents()
    {
        Debug.Log("csv count" + questCSV.Count);
        for(int i=0; i<5; i++)
        {
            questTexts[i].text = " " + questCSV[i]["Quest"];
        }
    }

    void changeContents() //quest를 bird quest로 바꿈
    {
        for(int i=0; i<questTexts.Count; i++)
        {
            //questTexts[i].fontStyle = FontStyles.Normal;
            questTexts[i].fontStyle &= ~FontStyles.Strikethrough;
            //Debug.Log("여기 되냐고");
        }

        questTexts[0].text = " ";
        questTexts[1].text = " " + questCSV[5]["Quest"];
        questTexts[2].text = " " + questCSV[6]["Quest"];
        questTexts[3].text = " " + questCSV[7]["Quest"];
        questTexts[4].text = " ";
    }

    void showQuestPage()
    {
        if(questPageIsOn == true)
        {
            QuestImage.SetActive(true);
            questPageIsOn = true;
        }
        else
        {
            QuestImage.SetActive(false);
            questPageIsOn = false;
        }
    }

    void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(questPageIsOn)
            {
                questPageIsOn = false;
            }
            else
            {
                questPageIsOn = true;
            }
        }
    }

    public void diedEnemyPlus()
    {
        diedEnemyCount++;
        //checkHeartQ[0] = true;
        //changeFinishedQuestUI(0);
    }

    public void sellApplePlus()
    {
        sellAppleCount++;

        //checkHeartQ[1] = true;
        //changeFinishedQuestUI(1);
    }

    public void sellHPItemPlus()
    {
        sellHPPlusItemCount++;
    }

    public void diedTreePlus()
    {
        deathTreesCount++;
    }

    public void plantedTreePlus()
    {
        plantedTreesCount++;
    }

    public void plantedTreeForBird()
    {
        plantedBirdTreesCount++;
    }

    void changeFinishedQuestUI(int questNum)
    {
        for(int i=0; i<checkHeartQ.Length; i++)
        {
            if(checkHeartQ[i] == true)
            {
                questTexts[i].fontStyle |= FontStyles.Strikethrough;
                questTexts[i].fontMaterial.SetFloat("_UnderlineWidthMultiplier", 5.0f);
            }
        }
    }

    void changeFinishQuestUIForBird(int questNum)
    {
        //for(int i=0; i<checkBirdQ.Length; i++)
        //{
        //    if(checkBirdQ[i]==true)
        //    {
        //        questTexts[i+1].fontStyle |= FontStyles.Strikethrough;
        //        questTexts[i+1].fontMaterial.SetFloat("_UnderlineWidthMultiplier", 5.0f);
        //    }
        //}

        //왜 커밋이 안될까?
        questTexts[questNum].fontStyle |= FontStyles.Strikethrough;
        questTexts[questNum].fontMaterial.SetFloat("_UnderlineWidthMultiplier", 5.0f);
    }

    void checkHeartQuestFinished()
    {
        //catch 5 monsters
        if (diedEnemyCount == 1)
        {
            checkHeartQ[0] = true;
            changeFinishedQuestUI(0);
        }
        //sell 2 apples
        if (sellAppleCount == 1)
        {
            checkHeartQ[1] = true;
            changeFinishedQuestUI(1);
        }
        //sell 1 hp plus item
        if (sellHPPlusItemCount == 1)
        {
            checkHeartQ[2] = true;
            changeFinishedQuestUI(2);
        }
        //cut down 3 trees
        if (deathTreesCount == 1)
        {
            checkHeartQ[3] = true;
            changeFinishedQuestUI(3);
        }
        //plant a tree
        if (plantedTreesCount == 1)
        {
            checkHeartQ[4] = true;
            changeFinishedQuestUI(4);
        }
    }

    void checkBirdQuestFinished()
    {
        //plant 3 trees
        if(plantedBirdTreesCount == 1)
        {
            checkBirdQ[0] = true;
            //0번은 비어있어서
            changeFinishQuestUIForBird(1);
        }
    }
}
