using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public string itemName; //�������� �̸�
    //public ItemType itemType; //������ ����
    public Sprite itemImage; //�������� �̹���(�κ��丮 �ȿ��� ���)
    public GameObject itemPrefab; //�������� ������(������ ������ ���������� ��)

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
            if (this.gameObject.tag == "HPPlusItem")
            {
                HPPlus();
                Destroy(gameObject);
            }
            if (this.gameObject.tag == "HPMinusItem")
            {
                HPMinus();
                Destroy(gameObject);
            }
        }
    }

    void HPPlus()
    {
        float health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        health = health + 2f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(health);
    }

    void HPMinus()
    {
        float health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        health = health - 2f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(health);
    }

    void DamagePlus()
    {
        float power = GameObject.FindWithTag("Player").GetComponent<Player>().getPower();
        power = power + 2f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setPower(power);
    }

    void DamageMinus()
    {
        float power = GameObject.FindWithTag("Player").GetComponent<Player>().getPower();
        power -= 0.5f;
        GameObject.FindWithTag("Player").GetComponent<Player>().setPower(power);
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(5);
    }

}
