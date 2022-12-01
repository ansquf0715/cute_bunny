using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    Inventory inventory;
    Player player;

    public string itemName; //아이템의 이름
    public Sprite itemImage; //아이템의 이미지(인벤토리 안에서 띄울)
    public GameObject itemPrefab; //아이템의 프리팹(아이템 생성시 프리팹으로 찍어냄)

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<Player>();
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
                inventory.PutItems(this);
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "DamageMinusItem")
            {
                inventory.PutItems(this);
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "HPPlusItem")
            {
                inventory.PutItems(this);
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "HPMinusItem")
            {
                inventory.PutItems(this);
                Destroy(gameObject);
            }
        }
    }

    public void HPPlus()
    {
        player.setHealth(2f);
        Debug.Log("HP Plus Item");

    }

    public void HPMinus()
    {
        player.setHealth(-2f);
        Debug.Log("HP Minus Item");

    }

    public void DamagePlus()
    {
        player.setPower(2f);
        StartCoroutine(delayTime());
        player.setPower(-2f);
        Debug.Log("Damage Plus");
    }

    public void DamageMinus()
    {
        player.setPower(-2f);
        StartCoroutine(delayTime());
        player.setPower(2f);
        Debug.Log("Damage Minus");

    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(10f);
    }

}
