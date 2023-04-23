using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellingBox : MonoBehaviour
{
    private Text MoneyText;
    BoxCollider rangeCollider;
    static public Rect baseSellRect;

    [SerializeField]
    private GameObject SellBoxBase;
    static public bool sellBoxActivated = false;

    static public int money = 0;
    static public int AppleMoney = 10;
    static public int GrapeMoney = 10;
    static public int OrangeMoney = 10;
    static public int PeachMoney = 5;
    static public int PlumMoney = 5;
    static public int RaspberryMoney = 3;
    static public int SeedMoney = 30;

    static public int DamagePlusItemMoney = 15;
    static public int DamageMinusItemMoney = 50;
    static public int HPPlusItemMoney = 12;
    static public int HPMinusItemMoney = 40;

    static public int checkSellAppleCount = 0;
    static public int checkSellHPPlusCount = 0;

    static bool checkAppleQuest = false;
    static bool checkHPPlusQuest = false;

    public static bool moneyToEscape = false;
    bool moneyToEscapeIsChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        MoneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        SellBoxBase.SetActive(false);
        GameObject rectParent = GameObject.FindWithTag("SellBox");
        baseSellRect = rectParent.transform.GetChild(0).GetComponent<RectTransform>().rect;
        //baseSellRect = transform.Find("SellBoxInside").gameObject.SetActive(false);
        rangeCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        SetMoneyText();

        if(!moneyToEscapeIsChanged)
        {
            if (checkMoney()) //돈이 있으면
            {
                Debug.Log("check money true");
                moneyToEscape = true;
                moneyToEscapeIsChanged = true;
            }
        }
    }

    bool checkMoney() //돈 있는지 확인
    {
        if(money >= 100)
        {
            return true;
        }
        return false;
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

    static public void SellThings(string itemName, int temCount)
    {
        switch(itemName)
        {
            case "Apple":
                money += AppleMoney * temCount;
                checkSellAppleCount += temCount;
                if (checkSellAppleCount >= 1 && !checkAppleQuest ) //heart quest 2 : sell 2 apples
                {
                    //Debug.Log("check sell apple count" + checkSellAppleCount);
                    //Debug.Log("이거 몇번 호출 됨?");
                    FindObjectOfType<QuestManager>().sellApplePlus();
                    checkAppleQuest = true;
                }
                break;
            case "Grape":
                money += GrapeMoney * temCount;
                break;
            case "Orange":
                money += OrangeMoney * temCount;
                break;
            case "Peach":
                money += PeachMoney * temCount;
                break;
            case "Plum":
                money += PlumMoney * temCount;
                break;
            case "Raspberry":
                money += RaspberryMoney * temCount;
                break;
            case "seed":
                money += SeedMoney * temCount;
                break;
            case "DamagePlusItem":
                money += DamageMinusItemMoney * temCount;
                break;
            case "DamageMinusItem":
                money += DamageMinusItemMoney * temCount;
                break;
            case "HPPlusItem":
                money += HPPlusItemMoney * temCount;
                checkSellHPPlusCount += temCount;
                if (checkSellHPPlusCount >= 1 && !checkHPPlusQuest) //heart quest 3: sell 1 hp plus item
                {
                    FindObjectOfType<QuestManager>().sellHPItemPlus();
                    checkHPPlusQuest = true;
                }
                break;
            case "HPMinusItem":
                money += HPMinusItemMoney * temCount;
                break;
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

    void SetMoneyText()
    {
        MoneyText.text = money.ToString() + "$";
    }
}
