using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFruit : MonoBehaviour
{
    public float fruit1health;

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
        if (other.gameObject.tag == "Player")
        {
            if (this.gameObject.tag == "Fruit1")
            {
                Fruit1Effect();

                int fruitcount = GameObject.FindWithTag("Player").GetComponent<Player>().getFruitCount();
                fruitcount++;
                GameObject.FindWithTag("Player").GetComponent<Player>().setFruitCount(fruitcount);
                
                Destroy(gameObject);
            }
        }
    }

    void Fruit1Effect() //°úÀÏ 1
    {
        float player_health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        player_health += fruit1health;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(player_health);
    }
}
