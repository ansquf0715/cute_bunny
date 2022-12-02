using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class TreeFruit : MonoBehaviour
{
    Inventory inventory;
    Player player;

    public string fruitName; // ���� �̸�
    public Sprite fruitImage; //���� �̹��� ( �κ��丮 �ȿ��� ���)
    public GameObject fruitPrefab; //�������� ������ (���� ������ ���������� ��)

    private float AppleHealth;
    private float GrapeHealth;
    private float OrangeHealth;
    private float PeachHealth;
    private float PlumHealth;
    private float RaspberryHealth;

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<Player>();

        AppleHealth = 1;
        GrapeHealth = 1;
        OrangeHealth = 1;
        PeachHealth = 1;
        PlumHealth = 1;
        RaspberryHealth = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (this.gameObject.tag == "Apple")
            {
                inventory.PutFruits(this);
                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Grape")
            {
                inventory.PutFruits(this);
                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Orange")
            {
                inventory.PutFruits(this);
                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Peach")
            {
                inventory.PutFruits(this);
                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Plum")
            {
                inventory.PutFruits(this);
                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Raspberry")
            {
                inventory.PutFruits(this);
                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Seed")
            {
                inventory.PutFruits(this);
                Destroy(gameObject);
            }
        }
    }

    public void AppleEffect()
    {
        player.setHealth(AppleHealth);
        //Debug.Log("Apple Effect");
    }

    public void GrapeEffect()
    {
        player.setHealth(GrapeHealth);
    }

    public void OrangeEffect()
    {
        player.setHealth(OrangeHealth);
    }

    public void PeachEffect()
    {
        player.setHealth(PeachHealth);
    }

    public void PlumEffect()
    {
        player.setHealth(PlumHealth);
    }

    public void RaspberryEffect()
    {
        player.setHealth(RaspberryHealth);
    }

    public void SeedEffect()
    {
        //Debug.Log("seed Effect ȣ��");
        FindObjectOfType<Player>().setUseSeed();
        //Debug.Log("make Tree ȣ��");
        FindObjectOfType<Player>().makeTree();
        //return;
    }
}
