using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    float damage;

    // Start is called before the first frame update
    void Start()
    {
        //damage = GameObject.FindWithTag("Player").GetComponent<Player>().getDamage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (this.gameObject.tag == "DamagePlusItem")
            {
                DamagePlus();
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "DamageMinusItem")
            {
                DamageMinus();
                Destroy(gameObject);
            }
        }
    }

    void OriginalDamage()
    {

    }
    void DamagePlus()
    {
        float damage = GameObject.FindWithTag("Player").GetComponent<Player>().getDamage();
        damage = damage + 2f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setDamage(damage);
    }

    void DamageMinus()
    {
        float damage = GameObject.FindWithTag("Player").GetComponent<Player>().getDamage();
        damage -= 0.5f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setDamage(damage);
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(5);
    }

}
