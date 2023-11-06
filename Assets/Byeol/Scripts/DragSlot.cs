using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        setColor();
    }

    public void setColor()
    {
        Color color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        imageItem.color = color;
    }
    public void setColorWhite()
    {
        Color color = imageItem.color;
        color.a = 0f;
        imageItem.color = color;

    }

}
