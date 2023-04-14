using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    List<Dictionary<string, object>> questCSV;

    public GameObject QuestImage;

    List<Text> questTexts = new List<Text>(); //quest 출력
    List<string> questContents = new List<string>(); //quest 내용

    public bool questPageIsOn = false;
 
    // Start is called before the first frame update
    void Start()
    {
        questCSV = CSVReader.Read("Quest");

        //this.gameObject.SetActive(false);
        QuestImage.SetActive(false);

        Text[] texts = QuestImage.GetComponentsInChildren<Text>();
        for(int i=0; i<texts.Length; i++)
        {
            questTexts.Add(texts[i]);
        }

        //for(int i=0; i<5; i++)
        //{
        //    questTexts.Add(QuestImage.GetComponentInChildren<Text>());
        //    //Debug.Log("quest text" + i + questTexts[i]);
        //}

        showContents();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        showQuestPage();
    }

    void showContents()
    {
        for(int i=0; i<questCSV.Count; i++)
        {
            questTexts[i].text = " " + questCSV[i]["Quest"];
            //Debug.Log("quest text" + questTexts[i].text);
        }
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
                //QuestImage.SetActive(false);
                questPageIsOn = false;
            }
            else
            {
                //QuestImage.SetActive(true);
                questPageIsOn = true;
            }
        }
    }
}
