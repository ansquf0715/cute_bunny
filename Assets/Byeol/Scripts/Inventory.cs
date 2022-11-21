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

    //나중에 오브젝트 풀린같은걸로 바꿔라
    private Slot[] slots = new Slot[10];

    private bool[] CheckSlotFull = new bool[10]; //slot이 비었는지 확인하는 배열
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
        //Debug.Log("비어있을 때 " + slots[1].item == null);
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
        //Debug.Log("나 들어있음?" + slots[0].item);
    }

    private void TryOpenInventory() //tab 누르면 inventory 화면 띄우기
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

    private void OpenInventory() //인벤토리 열기
    {
        InventoryBase.SetActive(true);
    }

    private void CloseInventory() //인벤토리 닫기
    {
        InventoryBase.SetActive(false);
    }

    private int checkFull() //slot 칸이 비었는지 안비었는지 확인하는 함수
    {
        for (int i = 0; i < CheckSlotFull.Length; i++)
        {
            if (CheckSlotFull[i] == false) //만약 빈칸이면
            {
                return i; //인덱스를 return 하고
            }
        }
        return 15; //아니라면 쓰레기값 -> 예외처리 해야함
    }

    public int isExist(Items _item, int _count = 1) //슬롯에 이미 존재하는 아이템인지 확인
    {
        for(int i=0; i<slots.Length; i++)
        {
            //Debug.Log("나 널아님 " + slots[i].item);
            Debug.Log("그만할래요 " + _item);
            if (slots[i].item.itemName == _item.itemName)
            {
                return i;
            }
            //if (slots[i].item != null)
            //{
            //    Debug.Log("나 널아님 " + slots[i].item);
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
            Debug.Log("그만할래요 " + _item.itemName);
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
