using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingZone : MonoBehaviour
{
    public GameObject[] enemies;
    public Vector3 spawn;

    int deathCount;

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
        int random_tree = 1;

        if(deathCount == random_tree) //죽은 나무 수가 random_death 값이랑 같아지면
        {
            Spawn();
            deathCount = 0;
        }
    }

    public void countDeath() //죽은 나무 수
    {
        deathCount++;
    }

    void Spawn()
    {
        int selection = Random.Range(0, enemies.Length);
        GameObject selectedPrefab = enemies[selection];
        //Vector3 spawnPos = spawn + new Vector3(-3.0f, 0f, 0f);

        GameObject instance = Instantiate(selectedPrefab, spawn, 
            Quaternion.identity * selectedPrefab.transform.localRotation);
    }
}
