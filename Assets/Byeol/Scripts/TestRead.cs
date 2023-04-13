using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRead : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> talk = CSVReader.Read("npcTalk");

        for (int i=0; i<talk.Count; i++)
        {
            Debug.Log("npc talking :" + talk[i]["messageNum"] + "first talk" + talk[i]["message"]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
