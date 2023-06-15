using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    Quest quest;
    QuestUI questUI;

    // Start is called before the first frame update
    void Start()
    {
        quest = new Quest();
        questUI = FindObjectOfType<QuestUI>();

        SellingBox sellingbox = FindObjectOfType<SellingBox>();
        sellingbox.SetQuest(quest);
        Player player = FindObjectOfType<Player>();
        player.SetQuest(quest);
        Enemy enemy = FindObjectOfType<Enemy>();
        enemy.SetQuest(quest);
        FightingZone fightingzone = FindObjectOfType<FightingZone>();
        fightingzone.SetQuest(quest);

        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.SetQuest(quest);

        HiddenKey hiddenKey = FindObjectOfType<HiddenKey>();
        hiddenKey.SetQuest(quest);

        quest.RegisterObserver(questUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
