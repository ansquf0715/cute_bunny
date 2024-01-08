using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemEffect
{
    public string itemName; //�������� �̸�(key ������ ����� ��)
}

public class ItemEffectDatabase : MonoBehaviour
{
    private Text seedInfo;

    string itemname;

    Items items;
    TreeFruit fruits;
    Player player;

    [SerializeField]
    private ItemEffect[] itemEffects;

    private float AppleHealth;
    private float GrapeHealth;
    private float OrangeHealth;
    private float PeachHealth;
    private float PlumHealth;
    private float RaspberryHealth;

    // Start is called before the first frame update
    void Start()
    {
        seedInfo = GameObject.Find("SeedInfo").GetComponent<Text>();

        items = FindObjectOfType<Items>();
        fruits = FindObjectOfType<TreeFruit>();
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

    public void UseItem(string itemName)
    {
        switch(itemName)
        {
            case "DamagePlusItem":
                DamagePlus();
                Invoke("replaceDamagePlus",15f);
                break;
            case "DamageMinusItem":
                DamageMinus();
                Invoke("replaceDamageMinus", 15f);
                break;
            case "HPPlusItem":
                HPPlus();
                break;
            case "HPMinusItem":
                HPMinus();
                break;
            case "Apple":
                AppleEffect();
                break;
            case "Grape":
                GrapeEffect();
                break;
            case "Orange":
                OrangeEffect();
                break;
            case "Peach":
                PeachEffect();
                break;
            case "Plum":
                PlumEffect();
                break;
            case "Raspberry":
                RaspberryEffect();
                break;
            case "seed":
                seedInfo.text = "PLANT A TREE!";
                SeedEffect();
                break;
        }

    }

    void replaceDamagePlus()
    {
        player.setPower(-2f);
    }

    void replaceDamageMinus()
    {
        player.setPower(2f);
    }

    //IEnumerator DelayReset()
    //{
    //    yield return new WaitForSeconds(5);
    //}
    public void AppleEffect()
    {
        Debug.Log("Apple Effect");
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
        Debug.Log("seed Effect ȣ��");
        FindObjectOfType<Player>().setUseSeed();
        FindObjectOfType<Player>().makeTree();
    }

    public void HPPlus()
    {
        player.setHealth(2f);
    }

    public void HPMinus()
    {
        player.minusPlayerHealth(2f);
        //Debug.Log("hp minus item");
    }

    public void DamagePlus()
    {
        player.setPower(2f);
    }

    public void DamageMinus()
    {
        player.setPower(-2f);
    }

}
