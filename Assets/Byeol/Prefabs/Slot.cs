using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler,
    IBeginDragHandler, 
    IEndDragHandler, IDropHandler , IDragHandler
{
    private Rect baseRect; // Inventory_Base �̹����� Rect ���� �޾� ��.
    public int temCount; //ȹ���� �������� ����

    public Items item; //ȹ���� ������
    public Image itemImage; // �������� �̹���
    public string itemName;

    public TreeFruit fruit; //ȹ���� ����
    //private int fruitCount; //ȹ���� ������ ����
    public Image fruitImage; //������ �̹���
    public string fruitName;

    public int temType = 0; //1�̸� item, 2�� fruit

    [SerializeField]
    private Text text_Count;

    private ItemEffectDatabase itemEffectDatabase;

    // Start is called before the first frame update
    void Start()
    {
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        itemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            int type = checkItemOrFruit();

            if (type == 1)
            {
                Debug.Log("type 1");
                itemEffectDatabase.UseItem(itemName);
                ChangeCount();

            }
            if (type == 2)
            {
                itemEffectDatabase.UseItem(fruitName);
                ChangeCount();
            }
            if (type == 0)
            {
                //Debug.Log("type�� seed");
                itemEffectDatabase.UseItem(fruitName);
                ChangeCount();
            }
        }
    }

    //���콺 �巡�װ� ���� ���� �� �߻��ϴ� �̺�Ʈ
    public void OnBeginDrag(PointerEventData eventData)
    {
        //int type = checkItemOrFruit();

        if(this.itemName != null)
        {
            //Debug.Log("item���� Ȯ��" + this.itemName);
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }

        if(this.itemName == null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(fruitImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    //���콺 �巡�� ���� �� ��� �߻��ϴ� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    //���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragSlot.instance.transform.localPosition.x < baseRect.xMin
            || DragSlot.instance.transform.localPosition.x > baseRect.xMax
            || DragSlot.instance.transform.localPosition.y < baseRect.yMin
            || DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            if(SellingBox.checkSellBox() == true)
            {
                Debug.Log("check item or fruit" + checkItemOrFruit());
                if (checkItemOrFruit() == 1)
                    SellingBox.SellThings(this.itemName, temCount);
                if (checkItemOrFruit() == 2)
                    SellingBox.SellThings(this.fruitName, temCount);
                //SellingBox.SellThings(this.fruitName, temCount);
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.setColorWhite();
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.setColorWhite();
                DragSlot.instance.dragSlot = null;
            }

        }
    }


    //�ش� ���Կ� ���𰡰� ���콺 ��� ���� �� �߻��ϴ� �̺�Ʈ
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeCount();
        }
    }

    private int checkItemOrFruit() // item�� ��� 1�� fruit�� ��� 2�� ����
    {
        if (temType == 1) //item
        {
            return 1;
        }
        else if (temType == 2) //fruit
        {
            if (this.fruitName == "seed")
            {
                Debug.Log("check Item Fruit Seed�� �ɸ�");
                return 0;
            }
            return 2;
        }
        else
            return -1;
    }

    private void setColor() //������ �̹����� ������ �����ϱ� ����
    {
        Color color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        fruitImage.color = color;
        itemImage.color = color;
    }

    public void AddItem(Items _item, int _count = 1)
    {
        item = _item;
        temCount = _count;
        itemImage.sprite = item.itemImage;
        itemName = _item.itemName;
        text_Count.text = temCount.ToString();
        temType = 1; //item �̶�� ����
        //Debug.Log("item temType" + temType);

        setColor();
    }

    public void AddFruit(TreeFruit _fruit, int _count = 1)
    {
        fruit = _fruit;
        temCount = _count;
        fruitImage.sprite = fruit.fruitImage;
        fruitName = _fruit.fruitName;
        text_Count.text = temCount.ToString();
        temType = 2; //fruit�̶�� ����
        //Debug.Log("fruit temType" + temType);

        if(fruitName == "seed") //�ִ°� �����̸�
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().setSeedCount();
        }
        setColor();
    }
    
    public void SetSlotCount(int _count = 1)
    {
        temCount += _count;
        if (fruitName == "seed")
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().setSeedCount();
        }
        text_Count.text = temCount.ToString();
    }

    private void ClearSlot()
    {
        item = null;
        temCount = 0;
        itemImage.sprite = null;
        text_Count.text = " ";

        FindObjectOfType<Inventory>().checkClear(this);

        Color color = itemImage.color;
        color.a = 0f;
        itemImage.color = color;
    }

    public void ChangeCount()
    {
        if (temCount > 1)
            SetSlotCount(-1);
        else if (temCount == 1)
            ClearSlot();
    }

    //private void ChangeSlot()
    //{
    //    Items _tempItem = item;
    //    int _tempItemCount = temCount;
    //    //string _tempItemName = itemName;

    //    AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.temCount);

    //    if (_tempItem != null)
    //        DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
    //    else
    //        //ClearSlot??????
    //        DragSlot.instance.dragSlot.ClearSlot();
    //}

    //void afterDrop()
    //{
    //    Instantiate(DragSlot.instance.dragSlot.item.itemPrefab,
    //        GameObject.FindWithTag("Player").GetComponent<Player>().getPos(),
    //        Quaternion.identity
    //        );
    //    //Instantiate(_prefab,
    //    //    GameObject.FindWithTag("Player").GetComponent<Player>().getPos(),
    //    //    Quaternion.identity
    //    //    );
    //    DragSlot.instance.dragSlot.SetSlotCount(-1);
    //    Debug.Log("SetSlotCount �ҷ�����?");

    //    DragSlot.instance.dragSlot = null;
    //}

}
