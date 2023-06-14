using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    List<Dictionary<string, object>> questCSV;

    public GameObject QuestImage;

    List<TextMeshProUGUI> questTexts = new List<TextMeshProUGUI>();

    public bool questPageIsOn = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void showContents()
    {
        for(int i=0; i<5; i++)
        {
            questTexts[i].text = " " + questCSV[i]["Quest"];
        }
    }
}
