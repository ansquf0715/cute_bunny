using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    public float Treehealth;
    Vector3 pos;
    private float damage;

    public GameObject fruit;
    BoxCollider rangeCollider; //������ ����Ʈ�� ��ġ�� ���ϱ� ����

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
                SpawnPos();
                Destroy(gameObject);
                dropFruit();
                GameObject.Find("FightingZone").GetComponent<FightingZone>().countDeath();
                //gameObject.transform.parent.GetComponent<FightingZone>().spawn 
                //    = this.gameObject.transform.position;
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

    void dropFruit()
    {
        int maxFruit = 3;

        for(int i=0; i<maxFruit; i++)
        {
            Instantiate(fruit, RandomPosition(), Quaternion.identity);
        }
    }
    
    Vector3 RandomPosition()
    {
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        //range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        //range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);

        range_X = Random.Range(range_X - 5, range_X + 5);
        range_Z = Random.Range(range_Z - 5, range_Z + 5);

        Vector3 RandomPosition = new Vector3(range_X, 0.5f, range_Z);

        Vector3 SpawnPos = gameObject.transform.position + RandomPosition;
        //Debug.Log("SpawnPos : " + SpawnPos);
        return SpawnPos;
    }

}
