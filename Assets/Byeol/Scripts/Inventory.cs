using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject InventoryBase; //inventory base 이미지
    [SerializeField]
    private GameObject SlotsParent; //Slot들의 부모인 Grid Setting

    private Slot[] slots; //슬롯들 배열

    // Start is called before the first frame update
    void Start()
    {
        slots = SlotsParent.GetComponentInChildren<Slot>;
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count=1)
    {
        if(Item.ItemType.Equipment != _item.itemType)
        {
            for(int i=0; i<slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if(slots[i].item.itemName = _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for(int i=0; i<slots.Length; i++)
        {
            if(slots[i].item = null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
