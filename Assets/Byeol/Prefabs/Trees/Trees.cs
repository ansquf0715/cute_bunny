using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    public float Treehealth;
    Vector3 pos;
    private float damage;

    //public GameObject fruit;
    BoxCollider rangeCollider; //과일을 떨어트릴 위치를 정하기 위해

    public GameObject[] items; //tree에서 떨어트리는 아이템들

    // Start is called before the first frame update
    void Start()
    {
        pos = this.gameObject.transform.position;
        rangeCollider = gameObject.GetComponent<BoxCollider>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        damage = GameObject.FindWithTag("Player").GetComponent<Player>().getPower();

        if (other.gameObject.tag == "Bullet")
        {
            //Debug.Log(pos);
            Destroy(other.gameObject);
            Treehealth -= damage;
            if (Treehealth == 0)
            {
                //SpawnPos();
                Destroy(gameObject);
                dropFruit();
                GameObject.Find("FightingZone").GetComponent<FightingZone>().countDeath();

            }
        }
    }

    public Vector3 getPos()
    {
        return pos;
    }
    public void SpawnPos()
    {
        gameObject.transform.parent.GetComponent<FightingZone>().spawn
            = this.gameObject.transform.position;
    }


    
    Vector3 RandomPosition()
    {
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);

        Vector3 RandomPosition = new Vector3(range_X, 0.5f, range_Z);

        Vector3 SpawnPos = gameObject.transform.position + RandomPosition;
        //Debug.Log("SpawnPos : " + SpawnPos);
        return SpawnPos;
    }

    GameObject selectItem() //랜덤으로 떨어트릴 아이템 정하기
    {
        int selectedIndex = Random.Range(0, items.Length);
        GameObject selectedPrefab = items[selectedIndex];
        return selectedPrefab;
    }

    void dropFruit()
    {
        int randomItemCount = Random.Range(1, 4);
        for(int i=0; i<randomItemCount; i++)
        {
            Instantiate(selectItem(), RandomPosition(), Quaternion.identity);
        }
    }

    //void dropFruit()
    //{
    //    int maxFruit = 3;

    //    for (int i = 0; i < maxFruit; i++)
    //    {
    //        Instantiate(fruit, RandomPosition(), Quaternion.identity);
    //    }
    //}
}
