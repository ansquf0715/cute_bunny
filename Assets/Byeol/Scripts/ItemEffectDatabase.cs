using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect
{
    public string itemName; //�������� �̸�(key ������ ����� ��)
}

public class ItemEffectDatabase : MonoBehaviour
{
    string itemname;

    Items items;
    TreeFruit fruits;

    [SerializeField]
    private ItemEffect[] itemEffects;

    // Start is called before the first frame update
    void Start()
    {
        items = FindObjectOfType<Items>();
        fruits = FindObjectOfType<TreeFruit>();
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
                items.DamagePlus();
                //StartCoroutine(DelayReset());
                //items.replaceDamagePlus();
                break;
            case "DamageMinusItem":
                items.DamageMinus();
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
                Debug.Log("seed switch ��");
                fruits.SeedEffect();
                break;
        }

    }

    //IEnumerator DelayReset()
    //{
    //    yield return new WaitForSeconds(5);
    //}

}
