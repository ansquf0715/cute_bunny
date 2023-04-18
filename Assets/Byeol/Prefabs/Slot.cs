using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler,
    IBeginDragHandler, 
    IEndDragHandler, IDropHandler , IDragHandler
{
    private Rect baseRect; // Inventory_Base 이미지의 Rect 정보 받아 옴.
    public int temCount; //획득한 아이템의 개수

    public Items item; //획득한 아이템
    public Image itemImage; // 아이템의 이미지
    public string itemName;

    public TreeFruit fruit; //획득한 과일
    //private int fruitCount; //획득한 과일의 개수
    public Image fruitImage; //과일의 이미지
    public string fruitName;

    public int temType = 0; //1이면 item, 2면 fruit

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
                //Debug.Log("type은 seed");
                itemEffectDatabase.UseItem(fruitName);
                ChangeCount();
            }
        }
    }

    //마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        //int type = checkItemOrFruit();

        if(this.itemName != null)
        {
            //Debug.Log("item인지 확인" + this.itemName);
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

    //마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    //마우스 드래그가 끝났을 때 발생하는 이벤트
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


    //해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeCount();
        }
    }

    private int checkItemOrFruit() // item일 경우 1을 fruit일 경우 2를 리턴
    {
        if (temType == 1) //item
        {
            return 1;
        }
        else if (temType == 2) //fruit
        {
            if (this.fruitName == "seed")
            {
                Debug.Log("check Item Fruit Seed에 걸림");
                return 0;
            }
            return 2;
        }
        else
            return -1;
    }

    private void setColor() //아이템 이미지의 투명도를 조정하기 위해
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
        temType = 1; //item 이라고 저장
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
        temType = 2; //fruit이라고 저장
        //Debug.Log("fruit temType" + temType);

        if(fruitName == "seed") //넣는게 씨앗이면
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
    //    Debug.Log("SetSlotCount 불려지나?");

    //    DragSlot.instance.dragSlot = null;
    //}

}
