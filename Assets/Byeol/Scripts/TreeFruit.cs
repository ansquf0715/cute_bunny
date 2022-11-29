using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class TreeFruit : MonoBehaviour
{
    Inventory inventory;

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
            //GameObject.Find("Inventory").GetComponent<Inventory>().AcquireFruit(this);
            //inventory.AcquireFruit(this);

            if (this.gameObject.tag == "Apple")
            {
                AppleEffect();

                //int fruitcount = GameObject.FindWithTag("Player").GetComponent<Player>().getFruitCount();
                //fruitcount++;
                //GameObject.FindWithTag("Player").GetComponent<Player>().setFruitCount(fruitcount);

                inventory.PutFruits(this);

                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Grape")
            {
                GrapeEffect();

                //int fruitcount = GameObject.FindWithTag("Player").GetComponent<Player>().getFruitCount();
                //fruitcount++;
                //GameObject.FindWithTag("Player").GetComponent<Player>().setFruitCount(fruitcount);

                inventory.PutFruits(this);

                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Orange")
            {
                OrangeEffect();

                //int fruitcount = GameObject.FindWithTag("Player").GetComponent<Player>().getFruitCount();
                //fruitcount++;
                //GameObject.FindWithTag("Player").GetComponent<Player>().setFruitCount(fruitcount);

                inventory.PutFruits(this);

                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Peach")
            {
                PeachEffect();

                //int fruitcount = GameObject.FindWithTag("Player").GetComponent<Player>().getFruitCount();
                //fruitcount++;
                //GameObject.FindWithTag("Player").GetComponent<Player>().setFruitCount(fruitcount);

                inventory.PutFruits(this);

                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Plum")
            {
                PlumEffect();

                //int fruitcount = GameObject.FindWithTag("Player").GetComponent<Player>().getFruitCount();
                //fruitcount++;
                //GameObject.FindWithTag("Player").GetComponent<Player>().setFruitCount(fruitcount);
                
                inventory.PutFruits(this);

                Destroy(gameObject);
            }

            if(this.gameObject.tag == "Raspberry")
            {
                RaspberryEffect();

                //int fruitcount = GameObject.FindWithTag("Player").GetComponent<Player>().getFruitCount();
                //fruitcount++;
                //GameObject.FindWithTag("Player").GetComponent<Player>().setFruitCount(fruitcount);
                
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
        float player_health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        player_health += AppleHealth;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(player_health);
    }

    public void GrapeEffect()
    {
        float player_health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        player_health += GrapeHealth;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(player_health);
    }

    public void OrangeEffect()
    {
        float player_health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        player_health += OrangeHealth;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(player_health);
    }

    public void PeachEffect()
    {
        float player_health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        player_health += PeachHealth;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(player_health);
    }

    public void PlumEffect()
    {
        float player_health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        player_health += PlumHealth;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(player_health);
    }

    public void RaspberryEffect()
    {
        float player_health = GameObject.FindWithTag("Player").GetComponent<Player>().getHealth();
        player_health += RaspberryHealth;
        GameObject.FindWithTag("Player").GetComponent<Player>().setHealth(player_health);
    }

    public void SeedEffect()
    {

    }
}
