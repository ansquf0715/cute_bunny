using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingBox : MonoBehaviour
{

    BoxCollider rangeCollider;
    static public Rect baseSellRect;

    [SerializeField]
    private GameObject SellBoxBase;
    static public bool sellBoxActivated = false;

    static public int money = 0;
    static public int AppleMoney = 10;

    // Start is called before the first frame update
    void Start()
    {
        SellBoxBase.SetActive(false);
        GameObject rectParent = GameObject.FindWithTag("SellBox");
        baseSellRect = rectParent.transform.GetChild(0).GetComponent<RectTransform>().rect;
        //baseSellRect = transform.Find("SellBoxInside").gameObject.SetActive(false);
        rangeCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public bool checkSellBox()
    {
        if(sellBoxActivated == true)
        {
            if (DragSlot.instance.transform.localPosition.x > baseSellRect.xMin
            || DragSlot.instance.transform.localPosition.x < baseSellRect.xMax
            || DragSlot.instance.transform.localPosition.y > baseSellRect.yMin
            || DragSlot.instance.transform.localPosition.y < baseSellRect.yMax)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    static public void SellThings(string itemName)
    {
        if(itemName == "Apple")
        {
            money += AppleMoney;
            Debug.Log("»ç°ú °ª : " + money);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Trigger Enter");
            SellBoxBase.SetActive(true);
            sellBoxActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Trigger Enter");
            SellBoxBase.SetActive(false);
            sellBoxActivated = false;
        }
    }
}
