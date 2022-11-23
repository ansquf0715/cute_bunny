using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
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

        setColor();
    }
    
    public void SetSlotCount()
    {
        temCount++;
        //Debug.Log("템카운트 올라감");
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
