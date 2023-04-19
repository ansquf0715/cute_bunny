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

    //int unknownPotionCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        InventoryBase.SetActive(false);
        slots = SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();

        checkItemForBirdQuest();
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

    public void checkClear(Slot cleardSlot) //슬롯에 있는게 지워졌는지 확인?
    {
        int temp = 0;
        for(int i=0; i<slots.Length; i++)
        {
            if (slots[i] == cleardSlot)
                temp = i;
        }
        CheckSlotFull[temp] = false;
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

    public int ItemExist(Items _item, int _count = 1) //슬롯에 이미 존재하는 아이템인지 확인
    {
        //Debug.Log("isExist 불려짐");
        for (int i = 0; i < slots.Length; i++)
        {
            //Debug.Log("포문 도는 중");
            if (slots[i].itemName == _item.itemName)
            {
               // Debug.Log("i나옴");
                return i;
            }
        }
        //Debug.Log("는 -1");
        return -1;
    }

    public int FruitExist(TreeFruit _fruit, int _count = 1)
    {
        for(int i=0; i<slots.Length; i++)
        {
            if(slots[i].fruitName == _fruit.fruitName)
            {
                return i;
            }
        }
        return -1;
    }

    public void putItemExists(Items _item, int index, int _count = 1)
    {
        slots[index].SetSlotCount();
    }

    public void putFruitExists(TreeFruit _fruit, int index, int _count = 1)
    {
        slots[index].SetSlotCount();
    }

    public void AcquireItem(Items _item, int _count = 1)
    {
        int toPut = checkFull();
        slots[toPut].AddItem(_item);
        CheckSlotFull[toPut] = true;
    }
    public void AcquireFruit(TreeFruit _fruit, int _count = 1)
    {
        int toPut = checkFull();
        slots[toPut].AddFruit(_fruit);
        CheckSlotFull[toPut] = true;
    }

    public void PutItems(Items _item, int _count = 1)
    {
        int check = ItemExist(_item, _count);
        if(check != -1)
        {
            putItemExists(_item, check, _count);
        }
        else
        {
            AcquireItem(_item, _count);
        }
    }

    public void PutFruits(TreeFruit _fruit, int _count=1)
    {
        //Debug.Log("Put Fruits 함수");
        int check = FruitExist(_fruit, _count);
        if(check != -1)
        {
            putFruitExists(_fruit, check, _count);
        }
        else
        {
            AcquireFruit(_fruit, _count);
        }
    }

    public bool canCountSeed() //인벤에 씨앗이 있는지
    {
        //Debug.Log("can Count Seed");
        for(int i=0; i<slots.Length; i++)
        {
            if(slots[i].fruitName == "seed")
            {
                return true;
            }
        }
        return false;
    }

    public int getCountSeed() //seed 개수 가져오는 함수
    {
        if (canCountSeed() == true)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].fruitName == "seed")
                {
                    //Debug.Log("템카운트 " + slots[i].temCount);
                    return slots[i].temCount;
                }
            }
        }
        return 0;
        //return 0; //씨앗이 없을 때
    }

    public void setSeedCount()
    {
        //Debug.Log("Set Seed Count 호출됨");
        for(int i=0; i<slots.Length; i++)
        {
            if(slots[i].fruitName == "seed")
            {
                slots[i].ChangeCount();
            }
        }
    }

    void checkItemForBirdQuest()
    {
        //bird quest 진행중인지 확인
        if(QuestManager.doingBirdQ == true)
        {
            int unknownPotionCount = 0;

            foreach(Slot slot in slots)
            {
                if(slot.itemName == "DamageMinusItem"
                    || slot.itemName == "HPMinusItem") //check item name
                {
                    unknownPotionCount += slot.temCount;
                }
            }

            if(unknownPotionCount >= 2)
            {
                FindObjectOfType<QuestManager>().gotUnknownItemForBird();
                unknownPotionCount = 0;
            }
        }
    }
}
