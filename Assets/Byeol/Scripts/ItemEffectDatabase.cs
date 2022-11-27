using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect
{
    public string itemName; //아이템의 이름(key 값으로 사용할 것)
}

public class ItemEffectDatabase : MonoBehaviour
{
    Items items;

    [SerializeField]
    private ItemEffect[] itemEffects;

    // Start is called before the first frame update
    void Start()
    {
        items = FindObjectOfType<Items>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseItem(Items _item)
    {
        if(_item.itemName == "DamagePlusItem")
        {
            items.DamagePlus();
        }
        if(_item.itemName == "DamageMinusItem")
        {
            items.DamageMinus();
        }
        if(_item.itemName == "HPPlusItem")
        {
            items.HPPlus();
        }
        if(_item.itemName == "HPMinusItem")
        {
            items.HPMinus();
        }

    }

}
