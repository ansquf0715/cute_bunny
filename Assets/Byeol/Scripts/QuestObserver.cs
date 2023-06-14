using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestObserver
{
    void UpdateQuestProgress(string questName, int progress);
}

public class QuestObserver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
