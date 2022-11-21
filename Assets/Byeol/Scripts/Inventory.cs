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

    //���߿� ������Ʈ Ǯ�������ɷ� �ٲ��
    private Slot[] slots = new Slot[10];

    private bool[] CheckSlotFull = new bool[10]; //slot�� ������� Ȯ���ϴ� �迭
    private int[] SetButton = new int[10];

    // Start is called before the first frame update
    void Start()
    {
        //slots = new Slot[10];
        //for (int i = 0; i < slots.Length; i++)
        //{
        //    slots[i] = SlotsParent.transform.GetChild(i).GetComponent<Slot>();
        //}

        slots = SlotsParent.GetComponentsInChildren<Slot>();

        //Debug.Log("Slots : " + slots.Length);
        //Debug.Log("������� �� " + slots[1].item == null);
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
        //Debug.Log("�� �������?" + slots[0].item);
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

    private int checkFull() //slot ĭ�� ������� �Ⱥ������ Ȯ���ϴ� �Լ�
    {
        for (int i = 0; i < CheckSlotFull.Length; i++)
        {
            if (CheckSlotFull[i] == false) //���� ��ĭ�̸�
            {
                return i; //�ε����� return �ϰ�
            }
        }
        return 15; //�ƴ϶�� �����Ⱚ -> ����ó�� �ؾ���
    }

    public int isExist(Items _item, int _count = 1) //���Կ� �̹� �����ϴ� ���������� Ȯ��
    {
        for(int i=0; i<slots.Length; i++)
        {
            //Debug.Log("�� �ξƴ� " + slots[i].item);
            Debug.Log("�׸��ҷ��� " + _item);
            if (slots[i].item.itemName == _item.itemName)
            {
                return i;
            }
            //if (slots[i].item != null)
            //{
            //    Debug.Log("�� �ξƴ� " + slots[i].item);
            //    if (slots[i].item.itemName == _item.itemName)
            //    {
            //        return i;
            //    }
            //}
        }
        return -1;
    }

    public void putExists(Items _item, int index, int _count = 1)
    {
        //for(int i=0; i<slots.Length; i++)
        //{
        //    if(slots[i].item.itemName == _item.itemName)
        //    {
        //        //slots[i].SetSlotCount(_count);
        //        slots[i].SetSlotCount();
        //    }
        //}
        slots[index].SetSlotCount();
    }

    public void AcquireItem(Items _item, int _count = 1)
    {
        int toPut = checkFull();
        slots[toPut].AddItem(_item);
        CheckSlotFull[toPut] = true;
    }

    public void PutItems(Items _item, int _count = 1)
    {
        //if (isExist(_item, _count) == true)
        //{
        //    Debug.Log("It's isExist");
        //    putExists(_item, _count);
        //}
        if (isExist(_item, _count) != -1)
        {
            Debug.Log("�׸��ҷ��� " + _item.itemName);
            int temp = isExist(_item, _count);
            putExists(_item, temp, _count);
        }
        else
        {
            AcquireItem(_item, _count);
        }
    }

    public void AcquireFruit(TreeFruit _fruit, int _count = 1)
    {
        int toPut = checkFull();
        slots[toPut].AddFruit(_fruit);
        CheckSlotFull[toPut] = true;
        //slots[toPut].SetSlotCount(_count);
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
