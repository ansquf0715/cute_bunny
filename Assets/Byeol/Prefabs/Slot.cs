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

    [SerializeField]
    private Text text_Count;
    //[SerializeField]
    //private GameObject CountImage;


    // Start is called before the first frame update
    void Start()
    {
        //item = null;
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        //Debug.Log("Base Rect" + baseRect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log(item.itemName + "을 사용");
        }
    }

    //마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        DragSlot.instance.dragSlot = this;
        DragSlot.instance.DragSetImage(itemImage);
        DragSlot.instance.transform.position = eventData.position;

        //Debug.Log("Begin Drag");

    }

    //마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
        //Debug.Log("On Drag");
    }

    //마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        //DragSlot.instance.setColorWhite();
        //DragSlot.instance.dragSlot = null;

        if(DragSlot.instance.transform.localPosition.x < baseRect.xMin
            || DragSlot.instance.transform.localPosition.x > baseRect.xMax
            || DragSlot.instance.transform.localPosition.y < baseRect.yMin
            || DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            //afterDrop();
            //Debug.Log("after drop");
            DragSlot.instance.dragSlot.ClearSlot();

        }

        DragSlot.instance.setColorWhite();
        DragSlot.instance.dragSlot = null;

        //Debug.Log("On End Drag");
    }


    //해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
            
        }
        
        //Debug.Log("On Drop");

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

        setColor();
    }

    public void AddFruit(TreeFruit _fruit, int _count = 1)
    {
        fruit = _fruit;
        temCount = _count;
        fruitImage.sprite = fruit.fruitImage;
        fruitName = _fruit.fruitName;
        text_Count.text = temCount.ToString();

        if(fruitName == "seed") //넣는게 씨앗이면
        //if(this.gameObject.tag == "Seed")
        {
            //Debug.Log("이거 호출 되나?");
            GameObject.FindWithTag("Player").GetComponent<Player>().setSeedCount();
        }

        setColor();
    }
    
    public void SetSlotCount(int _count = 1)
    {
        temCount += _count;
        Debug.Log("카운트 " + temCount);
        //temCount++;
        //Debug.Log("템카운트 올라감");

        //if(fruitName == "Seed")
        if (fruitName == "seed")
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().setSeedCount();
            //Debug.Log("이거 호출 되나?");
        }
        text_Count.text = temCount.ToString();
    }

    private void ClearSlot()
    {
        item = null;
        temCount = 0;
        itemImage.sprite = null;

        Color color = itemImage.color;
        color.a = 0f;
        itemImage.color = color;
        //text_Count.text = "0";
    }

    private void ChangeSlot()
    {
        Items _tempItem = item;
        int _tempItemCount = temCount;
        //string _tempItemName = itemName;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.temCount);

        if (_tempItem != null)
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            //ClearSlot??????
            DragSlot.instance.dragSlot.ClearSlot();
    }

    void afterDrop()
    {
        Instantiate(DragSlot.instance.dragSlot.item.itemPrefab,
            GameObject.FindWithTag("Player").GetComponent<Player>().getPos(),
            Quaternion.identity
            );
        DragSlot.instance.dragSlot.SetSlotCount(-1);
        Debug.Log("SetSlotCount 불려지나?");

        DragSlot.instance.dragSlot = null;
    }

}
