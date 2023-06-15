using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingZone : MonoBehaviour
{
    static Quest quest;
    public void SetQuest(Quest questInstance)
    {
        quest = questInstance;
    }

    public GameObject[] enemies;
    public Vector3 spawn;

    int deathCount;
    bool checkTreeDeathQuest = false;

    // Start is called before the first frame update
    void Start()
    {
        deathCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RandomEnemy();
    }

    void RandomEnemy()
    {
        //int random_tree = Random.Range(2, 5);
        int random_tree = 3;

        if(deathCount == random_tree) //���� ���� ���� random_death ���̶� ��������
        {
            Spawn();
            deathCount = 0;
        }
    }

    public void countDeath() //���� ���� ��
    {
        deathCount++;
        if(deathCount >= 1)
        {
            quest.CompleteQuest("Cut down 3 trees!");
            Debug.Log("cut down 3 trees");
        }
    }

    void Spawn()
    {
        Debug.Log("Spawn");

        int selection = Random.Range(0, enemies.Length);
        GameObject selectedPrefab = enemies[selection];

        Vector3 spawnPos = spawn + new Vector3(-3.0f, 0f, 0f);

        GameObject instance = Instantiate(selectedPrefab, spawnPos, 
            Quaternion.identity * selectedPrefab.transform.localRotation);
    }

    public void setSpawnPos(Vector3 _pos)
    {
        spawn = _pos;
    }

}
