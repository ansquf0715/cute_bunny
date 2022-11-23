using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
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

        setColor();
    }
    
    public void SetSlotCount()
    {
        temCount++;
        //Debug.Log("��ī��Ʈ �ö�");
        text_Count.text = temCount.ToString();
    }

    private void ClearSlot()
    {
        item = null;
        temCount = 0;
        itemImage.sprite = null;

        //text_Count.text = "0";
    }

    //private void ClearSlot()
    //{
    //    item = null;
    //    itemCount = 0;
    //    itemImage.sprite = null;
    //    setColor(0);

    //    text_Count.text = "0";
    //    CountImage.SetActive(false);
    //}
}
