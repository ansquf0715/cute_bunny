using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Items item; //ȹ���� ������
    private int itemCount; //ȹ���� �������� ����
    public Image itemImage; // �������� �̹���

    //public TreeFruit fruit;
    public Image fruitImage;

    public GameObject fruit;

    //[SerializeField]
    //private Text text_Count;
    //[SerializeField]
    //private GameObject CountImage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setColor(float _alpha) //������ �̹����� ���� ����
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Items _item) // �� ���� �ҷ��ִ� �� ����
    {
        item = _item;
        itemImage.sprite = item.itemImage;
        Debug.Log("item! ");
        setColor(1);
    }

    //public void AddFruit(TreeFruit _fruit)
    //{
    //    fruit = _fruit;
    //    fruitImage.sprite = fruit.fruitImage;
    //    Debug.Log("Fruit! ");
    //    setColor(1);
    //}

    public void AddFruit(GameObject _fruit)
    {
        fruit = _fruit;
        fruitImage.sprite = GameObject.Find("TreeFruit").GetComponent<TreeFruit>().getFruitImage();
        Debug.Log("item Apple!");
        setColor(1);
    }

    //public void AddItem(Items _item, int _count = 1)
    //{
    //    item = _item;
    //    itemCount = _count;
    //    itemImage.sprite = item.itemImage;

    //    if(item.itemType != item.ItemType.Equipment)
    //    {
    //        CountImage.SetActive(true);
    //        text_Count.text = itemCount.ToString();
    //    }
    //    else
    //    {
    //        text_Count.text = "0";
    //        CountImage.SetActive(false);
    //    }

    //    setColor(1);
    //}

    ////�ش� ������ ������ ���� ������Ʈ
    //public void SetSlotCount(int _count)
    //{
    //    itemCount += _count;
    //    text_Count.text = itemCount.ToString();

    //    if (itemCount <= 0)
    //        ClearSlot();
    //}

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
