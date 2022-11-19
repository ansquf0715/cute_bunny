using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject InventoryBase; //inventory base �̹���
    [SerializeField]
    private GameObject SlotsParent; //Slot���� �θ��� Grid Setting

    private Slot[] slots; //���Ե� �迭

    // Start is called before the first frame update
    void Start()
    {
        //for(int i=0; i<slots.Length; i++)
        //{
        //    slots[i] = SlotsParent.GetComponentInChildren<Slot>();
        //}
        slots = SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory() //tab ������ inventory ȭ�� ����
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

    private void OpenInventory() //�κ��丮 ����
    {
        InventoryBase.SetActive(true);
    }

    private void CloseInventory() //�κ��丮 �ݱ�
    {
        InventoryBase.SetActive(false);
    }

    public void AcquireItem(Items _item, int _count = 1)
    {
        for(int i=0; i<slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                slots[i].AddItem(_item);
                return;
            }
        }
    }

    //public void AcquireFruit(TreeFruit _fruit, int _count = 1)
    //{
    //    for(int i=0; i<slots.Length; i++)
    //    {
    //        if(slots[i].item == null)
    //        {
    //            slots[i].AddFruit(_fruit);
    //            return;
    //        }
    //    }
    //}

    public void AcquireFruit(GameObject _fruit)
    {
        Debug.Log("Acquire Fruit!");

        for (int i=0; i<slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                slots[i].AddFruit(_fruit);
                return;
            }
        }

    }

    //public void AcquireItem(Item _item, int _count=1)
    //{
    //    if(Item.ItemType.Equipment != _item.itemType)
    //    {
    //        for(int i=0; i<slots.Length; i++)
    //        {
    //            if(slots[i].item != null)
    //            {
    //                if(slots[i].item.itemName = _item.itemName)
    //                {
    //                    slots[i].SetSlotCount(_count);
    //                    return;
    //                }
    //            }
    //        }
    //    }

    //    for(int i=0; i<slots.Length; i++)
    //    {
    //        if(slots[i].item = null)
    //        {
    //            slots[i].AddItem(_item, _count);
    //            return;
    //        }
    //    }
    //}
}
