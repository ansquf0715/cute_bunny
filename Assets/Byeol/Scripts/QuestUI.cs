using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour, IQuesetObserver
{
    public enum QuestPage
    {
        HeartQuest,
        BirdQuest,
    }

    QuestPage currentQuestPage = QuestPage.HeartQuest;

    public QuestPage GetCurrentQuestPage()
    {
        return currentQuestPage;
    }

    //Implement the interface methods
    public void NotifyQuestProgress(string questName, int progress)
    {

    }


    List<Dictionary<string, object>> questData;

    public GameObject QuestImage;
    List<TextMeshProUGUI> questTexts = new List<TextMeshProUGUI>();

    public bool questPageIsOn = false;

    private bool[] isQuestDone;
    int heartQuestCount;
    int birdQuestCount;

    bool canOpenQuestPage = true;

    // Start is called before the first frame update
    void Start()
    {
        questData = CSVReader.Read("Quest");
        QuestImage.SetActive(false);

        TextMeshProUGUI[] texts = QuestImage.GetComponentsInChildren<TextMeshProUGUI>();
        for(int i=0; i<texts.Length; i++)
        {
            questTexts.Add(texts[i]);
        }

        isQuestDone = new bool[questTexts.Count];
        heartQuestCount = 5;
        birdQuestCount = 3;

        showContents();
    }

    public void NotifyQuestCompletion(string questName)
    {
        // Mark the completed quest with strikethrough
        for (int i = 0; i < questTexts.Count; i++)
        {
            string trimmedQuestText = questTexts[i].text.Trim();
            if (trimmedQuestText.Equals(questName))
            {
                questTexts[i].fontStyle |= FontStyles.Strikethrough;
                questTexts[i].fontMaterial.SetFloat("_UnderlineWidthMultiplier", 5.0f);

                // Update the isQuestTextStrikethrough array
                isQuestDone[i] = true;

                if (currentQuestPage == QuestPage.HeartQuest)
                {
                    // Check if all heart quests are completed
                    bool allHeartQuestsCompleted = true;
                    for (int j = 0; j < questTexts.Count; j++)
                    {
                        if (!isQuestDone[j] && j < heartQuestCount)
                        {
                            allHeartQuestsCompleted = false;
                            break;
                        }
                    }

                    if (allHeartQuestsCompleted)
                    {
                        SwitchToBirdQuestPage();

                        //if (HeartNPC.heartCommunicationOver)
                        //{
                        //    SwitchToBirdQuestPage();
                        //    Debug.Log("여기 걸려주라 제발");
                        //}
                    }
                }
                else if (currentQuestPage == QuestPage.BirdQuest)
                {
                    // Check if all bird quests are completed
                    bool allBirdQuestsCompleted = true;
                    for (int j = heartQuestCount; j < questTexts.Count; j++)
                    {
                        if (!isQuestDone[j])
                        {
                            allBirdQuestsCompleted = false;
                            break;
                        }
                    }

                    if (allBirdQuestsCompleted)
                    {
                        // Handle completion of all bird quests
                        canOpenQuestPage = false;
                    }
                }

                return; 
            }
        }

        Debug.LogError("Quest text not found for QuestName: " + questName);
    }

    // Update is called once per frame
    void Update()
    {
        showQuestPage();
    }

    void showQuestPage()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canOpenQuestPage)
        {
            if (QuestImage.activeSelf)
                QuestImage.SetActive(false);
            else
                QuestImage.SetActive(true);
        }
    }

    void showContents()
    {
        int questIndex = 0;
        for (int i = 0; i < questData.Count; i++)
        {
            string npcName = questData[i]["NPC"].ToString();
            bool isHeartNPCQuest = npcName.Equals("HeartNPC",
                System.StringComparison.OrdinalIgnoreCase);

            if (isHeartNPCQuest)
            {
                string questText = questData[i]["Quest"].ToString();
                questTexts[questIndex].text = " " + questText;
            }
            questIndex++;

            if (questIndex >= questTexts.Count)
                break;
        }
    }

    private void SwitchToBirdQuestPage()
    {
        Debug.Log("switch to bird quest page");
        // Clear existing heart quest texts
        for (int i = 0; i < questTexts.Count; i++)
        {
            string trimmedQuestText = questTexts[i].text.Trim();
            bool isHeartQuest = questData[i]["NPC"].ToString().Equals("HeartNPC", System.StringComparison.OrdinalIgnoreCase);
            if (isHeartQuest)
            {
                questTexts[i].text = string.Empty;
                questTexts[i].fontStyle &= ~FontStyles.Strikethrough; // Remove strikethrough
            }
        }

        // Show bird quests
        int questIndex = 0;
        for (int i = 0; i < questData.Count; i++)
        {
            string npcName = questData[i]["NPC"].ToString();
            bool isBirdQuest = npcName.Equals("Sparrow", System.StringComparison.OrdinalIgnoreCase);

            if (isBirdQuest)
            {
                string questText = questData[i]["Quest"].ToString();
                questTexts[questIndex].text = " " + questText;
                questIndex++;
            }

            if (questIndex >= questTexts.Count)
                break;
        }

        currentQuestPage = QuestPage.BirdQuest;
    }

}
