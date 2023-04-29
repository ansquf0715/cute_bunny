using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    static public bool toCreateBoss = false;

    public GameObject boss;
    GameObject clonedBoss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        createBoss();
    }

    void createBoss()
    {
        if(toCreateBoss == true)
        {
            toCreateBoss = false;

            //clonedBoss =
            //    Instantiate(boss, new Vector3(-3.82f, 33.8f, 56.5f),
            //    Quaternion.Euler(0f, 180f, 0));

            clonedBoss =
                Instantiate(boss, new Vector3(-3.82f, 15f, 56.5f),
                Quaternion.Euler(0f, 180f, 0));
        }
    }
}
