using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StatePattern;
public class Inventory : MonoBehaviour
{
    static Quest quest;
    public void SetQuest(Quest questInstance)
    {
        quest = questInstance;
    }

    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject InventoryBase; 
    [SerializeField]
    private GameObject SlotsParent; 

    //���߿� ������Ʈ Ǯ�������ɷ� �ٲ��
    private Slot[] slots = new Slot[10];

    private bool[] CheckSlotFull = new bool[10]; 
    private int[] SetButton = new int[10];


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

    private void TryOpenInventory() 
    {
        if(!Boss.bossIsMoved)
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
    }

    private void OpenInventory() 
    {
        InventoryBase.SetActive(true);
    }

    private void CloseInventory() 
    {
        InventoryBase.SetActive(false);
    }

    public void checkClear(Slot cleardSlot) 
    {
        int temp = 0;
        for(int i=0; i<slots.Length; i++)
        {
            if (slots[i] == cleardSlot)
                temp = i;
        }
        CheckSlotFull[temp] = false;
    }

    private int checkFull() 
    {
        for (int i = 0; i < CheckSlotFull.Length; i++)
        {
            if (CheckSlotFull[i] == false) 
            {
                return i; 
            }
        }
        return 15; 
    }

    public int ItemExist(Items _item, int _count = 1) //���Կ� �̹� �����ϴ� ���������� Ȯ��
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName == _item.itemName)
            {
                return i;
            }
        }
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
        //Debug.Log("Put Fruits �Լ�");
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

    public bool canCountSeed() //�κ��� ������ �ִ���
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

    public int getCountSeed() 
    {
        if (canCountSeed() == true)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].fruitName == "seed")
                {
                    return slots[i].temCount;
                }
            }
        }
        return 0;
        //return 0; //������ ���� ��
    }

    public void setSeedCount()
    {
        //Debug.Log("Set Seed Count ȣ���");
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
        QuestUI questUI = FindObjectOfType<QuestUI>();
        if(questUI != null)
        {
            string page = questUI.GetCurrentQuestPage().ToString();
            if(page == "BirdQuest")
            {
                int unknownPotionCount = 0;

                foreach (Slot slot in slots)
                {
                    if (slot.itemName == "DamageMinusItem"
                        || slot.itemName == "HPMinusItem") //check item name
                    {
                        unknownPotionCount += slot.temCount;
                    }
                }

                if (unknownPotionCount >= 2)
                {
                    //FindObjectOfType<QuestManager>().gotUnknownItemForBird();
                    quest.CompleteQuest("You must have 2 unknown potions");
                    unknownPotionCount = 0;
                }
            }
        }
    }

}
