using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemEffect
{
    public string itemName; //아이템의 이름(key 값으로 사용할 것)
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

    // Start is called before the first frame update
    void Start()
    {
        seedInfo = GameObject.Find("SeedInfo").GetComponent<Text>();

        items = FindObjectOfType<Items>();
        fruits = FindObjectOfType<TreeFruit>();
        player = FindObjectOfType<Player>();
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
                //Debug.Log("Before item " + player.getPower());
                items.DamagePlus();
                Invoke("replaceDamagePlus",15f);
                //Debug.Log("After replace" + player.getPower());
                break;
            case "DamageMinusItem":
                items.DamageMinus();
                Invoke("replaceDamageMinus", 15f);
                break;
            case "HPPlusItem":
                items.HPPlus();
                //Debug.Log("HP PLus switch");
                break;
            case "HPMinusItem":
                items.HPMinus();
                //Debug.Log("HP Minus called");
                break;
            case "Apple":
                fruits.AppleEffect();
                //Debug.Log("Apple Called");
                break;
            case "Grape":
                fruits.GrapeEffect();
                break;
            case "Orange":
                fruits.OrangeEffect();
                break;
            case "Peach":
                fruits.PeachEffect();
                break;
            case "Plum":
                fruits.PlumEffect();
                break;
            case "Raspberry":
                fruits.RaspberryEffect();
                break;
            case "seed":
                seedInfo.text = "PLANT A TREE!";
                fruits.SeedEffect();
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

}
