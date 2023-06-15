using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenKey : MonoBehaviour
{
    static Quest quest;

    public void SetQuest(Quest questInstance)
    {
        quest = questInstance;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            quest.CompleteQuest("Find the key");
            Destroy(this.gameObject, 1f);
        }
    }
}
