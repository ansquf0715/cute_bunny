using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    public float Treehealth;
    Vector3 pos;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.gameObject.transform.position;
        damage = GameObject.FindWithTag("Player").GetComponent<Player>().getDamage();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            //Debug.Log(pos);
            Destroy(other.gameObject);
            Treehealth -= damage;
            if (Treehealth == 0)
            {
                SpawnPos();
                Destroy(gameObject);
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

}
