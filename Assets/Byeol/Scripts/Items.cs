using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    Inventory inventory;
    Player player;

    public string itemName; //�������� �̸�
    public Sprite itemImage; //�������� �̹���(�κ��丮 �ȿ��� ���)
    public GameObject itemPrefab; //�������� ������(������ ������ ���������� ��)

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
