using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private int temCount; //ȹ���� �������� ����

    public Items item; //ȹ���� ������
    public Image itemImage; // �������� �̹���

    public TreeFruit fruit; //ȹ���� ����
    private int fruitCount; //ȹ���� ������ ����
    public Image fruitImage; //������ �̹���

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
        text_Count.text = temCount.ToString();

        setColor();
    }

    public void AddFruit(TreeFruit _fruit)
    {
        fruit = _fruit;
        fruitImage.sprite = fruit.fruitImage;
        //Debug.Log("Fruit! ");
        setColor();
    }

    //public void SetSlotCount(int _count)
    //{
    //    //temCount += _count;
    //    temCount++;
    //    text_Count.text = temCount.ToString();

    //    if (temCount <= 0)
    //        ClearSlot();
    //}
    
    public void SetSlotCount()
    {
        temCount++;
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
