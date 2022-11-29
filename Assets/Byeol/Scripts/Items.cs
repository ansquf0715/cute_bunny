using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    Inventory inventory;

    public string itemName; //아이템의 이름
    public Sprite itemImage; //아이템의 이미지(인벤토리 안에서 띄울)
    public GameObject itemPrefab; //아이템의 프리팹(아이템 생성시 프리팹으로 찍어냄)

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
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
                //DamagePlus();
                inventory.PutItems(this);
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "DamageMinusItem")
            {
                //DamageMinus();
                inventory.PutItems(this);
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "HPPlusItem")
            {
                //HPPlus();
                inventory.PutItems(this);
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "HPMinusItem")
            {
                //HPMinus();
                inventory.PutItems(this);
                Destroy(gameObject);
            }
        }
    }

    public void HPPlus()
    {
        float health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        health = health + 2f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(health);
        Debug.Log("HP Plus Item");

    }

    public void HPMinus()
    {
        float health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        health = health - 2f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(health);
        Debug.Log("HP Minus Item");

    }

    public void DamagePlus()
    {
        float power = GameObject.FindWithTag("Player").GetComponent<Player>().getPower();
        power = power + 2f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setPower(power);
        Debug.Log("Damage Plus");
    }

    public void DamageMinus()
    {
        float power = GameObject.FindWithTag("Player").GetComponent<Player>().getPower();
        power -= 0.5f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setPower(power);
        Debug.Log("Damage Minus");

    }

    //IEnumerator delayTime()
    //{
    //    yield return new WaitForSeconds(5);
    //}

}
