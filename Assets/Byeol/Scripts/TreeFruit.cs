using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class TreeFruit : MonoBehaviour
{
    Inventory inventory;
    Player player;

    public string fruitName; // 과일 이름
    public Sprite fruitImage; //과일 이미지 ( 인벤토리 안에서 띄울)
    public GameObject fruitPrefab; //아이템의 프리팹 (과일 생성시 프리팹으로 찍어냄)

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

    }
}
