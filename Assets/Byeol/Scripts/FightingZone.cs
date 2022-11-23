using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingZone : MonoBehaviour
{
    public GameObject[] enemies;
    public Vector3 spawn;

    int deathCount;

    //BoxCollider rangeCollider;

    // Start is called before the first frame update
    void Start()
    {
        deathCount = 0;
        //rangeCollider = gameObject.GetComponent<BoxCollider>();
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

        if(deathCount == random_tree) //���� ���� ���� random_death ���̶� ��������
        {
            Spawn();
            deathCount = 0;
        }
    }

    public void countDeath() //���� ���� ��
    {
        deathCount++;
    }

    void Spawn()
    {
        int selection = Random.Range(0, enemies.Length);
        GameObject selectedPrefab = enemies[selection];
        Vector3 spawnPos = spawn + new Vector3(-3.0f, 0f, 0f);

        GameObject instance = Instantiate(selectedPrefab, spawnPos, 
            Quaternion.identity * selectedPrefab.transform.localRotation);
    }

    //public BoxCollider getBoxCollider()
    //{
    //    return rangeCollider;
    //}
}
