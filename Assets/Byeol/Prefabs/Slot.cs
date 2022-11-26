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
            Debug.Log(item.itemName + "�� ���");
        }
    }

    //���콺 �巡�װ� ���� ���� �� �߻��ϴ� �̺�Ʈ
    public void OnBeginDrag(PointerEventData eventData)
    {
        DragSlot.instance.dragSlot = this;
        DragSlot.instance.DragSetImage(itemImage);
        DragSlot.instance.transform.position = eventData.position;

        //Debug.Log("Begin Drag");

    }

    //���콺 �巡�� ���� �� ��� �߻��ϴ� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
        //Debug.Log("On Drag");
    }

    //���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        //DragSlot.instance.setColorWhite();
        //DragSlot.instance.dragSlot = null;

        if(DragSlot.instance.transform.localPosition.x < baseRect.xMin
            || DragSlot.instance.transform.localPosition.x > baseRect.xMax
            || DragSlot.instance.transform.localPosition.y < baseRect.yMin
            || DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }

        //���� ����
        //DragSlot.instance.setColorWhite(this);
        DragSlot.instance.dragSlot = null;

        //Debug.Log("On End Drag");
    }


    //�ش� ���Կ� ���𰡰� ���콺 ��� ���� �� �߻��ϴ� �̺�Ʈ
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
            Debug.Log("Change Slot");
        }

        Debug.Log("On Drop");

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

        setColor();
    }

    public void AddFruit(TreeFruit _fruit, int _count = 1)
    {
        fruit = _fruit;
        temCount = _count;
        fruitImage.sprite = fruit.fruitImage;
        fruitName = _fruit.fruitName;
        text_Count.text = temCount.ToString();

        if(fruitName == "seed") //�ִ°� �����̸�
        //if(this.gameObject.tag == "Seed")
        {
            //Debug.Log("�̰� ȣ�� �ǳ�?");
            GameObject.FindWithTag("Player").GetComponent<Player>().setSeedCount();
        }

        setColor();
    }
    
    public void SetSlotCount()
    {
        temCount++;
        //Debug.Log("��ī��Ʈ �ö�");

        //if(fruitName == "Seed")
        if (fruitName == "seed")
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().setSeedCount();
            //Debug.Log("�̰� ȣ�� �ǳ�?");
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
}
