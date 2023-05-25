using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StatePattern;

public class QuestManager : MonoBehaviour
{
    List<Dictionary<string, object>> questCSV;

    public GameObject QuestImage;

    List<TextMeshProUGUI> questTexts = new List<TextMeshProUGUI>();

    public bool questPageIsOn = false;

    //slot에서 불러와서 birdQ인지 확인함
    public bool changeToBirdQuest = false;
    public static bool doingBirdQ = false;

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
    public static int gotUnknownPotionCount;
    public static int foundKey;

    bool allQuestIsDone = false;

    // Start is called before the first frame update
    void Start()
    {
        questCSV = CSVReader.Read("Quest");
        QuestImage.SetActive(false);

        TextMeshProUGUI[] texts = QuestImage.GetComponentsInChildren<TextMeshProUGUI>();
        for(int i=0; i<texts.Length; i++)
        {
            questTexts.Add(texts[i]);
        }

        diedEnemyCount = 0;
        for (int i = 0; i < checkHeartQ.Length; i++)
            checkHeartQ[i] = false;
        //checkHeartQ[i] = true;

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
                doingBirdQ = true;
            }

            checkBirdQuestFinished();

            if(checkAllBirdQuestIsDone() && !allQuestIsDone) //bird quest를 다 했으면
            {
                FindObjectOfType<SparrowNPC>().birdQuestIsDone = true;
                allQuestIsDone = false;
            }
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

    bool checkAllBirdQuestIsDone()
    {
        for(int i=0; i<checkBirdQ.Length; i++)
        {
            if (checkBirdQ[i] == false)
                return false;
        }
        return true;
    }

    void showContents()
    {
        for(int i=0; i<5; i++)
        {
            questTexts[i].text = " " + questCSV[i]["Quest"];
        }
    }

    void changeContents() //quest를 bird quest로 바꿈
    {
        for(int i=0; i<questTexts.Count; i++)
        {
            questTexts[i].fontStyle &= ~FontStyles.Strikethrough;
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
        if(!Boss.bossIsMoved)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                if (questPageIsOn)
                    questPageIsOn = false;
                else
                    questPageIsOn = true;
            }
        }
    }

    public void diedEnemyPlus()
    {
        diedEnemyCount++;
    }

    public void sellApplePlus()
    {
        sellAppleCount++;
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

    public void gotUnknownItemForBird()
    {
        gotUnknownPotionCount++;
    }

    public void foundKeyForBird()
    {
        foundKey++;
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
        //Sell 2 unknown potions
        if(gotUnknownPotionCount == 1)
        {
            doingBirdQ = false;
            checkBirdQ[1] = true;
            changeFinishQuestUIForBird(2);
        }

        //Find the key
        if(foundKey == 1)
        {
            checkBirdQ[2] = true;
            changeFinishQuestUIForBird(3);
        }
    }

}
