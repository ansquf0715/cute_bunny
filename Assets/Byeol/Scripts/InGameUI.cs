using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private Text GameDay;

    private float dTime;
    private int day;
    private int month;
    private int year;

    // Start is called before the first frame update
    void Start()
    {
        GameDay = GameObject.Find("DayText").GetComponent<Text>();
        year = 1;
        month = 1;
        day = 1;
    }

    // Update is called once per frame
    void Update()
    {
        DayUpdate();
        MonthUpdate();
        YearUpdate();
        SetCountText();
    }

    void DayUpdate() //��¥ ������Ʈ
    {
        dTime += Time.deltaTime;
        if (dTime >= 300f) //5���� ������ �Ϸ簡 �������� ����
        {
            day++;
            dTime = 0;
        }
    }

    void MonthUpdate() //�� ������Ʈ
    {
        if (day == 31)
        {
            day = 1;
            month++;
        }
    }

    void YearUpdate() //���� ������Ʈ
    {
        if (month == 13)
        {
            month = 1;
            year++;
        }
    }

    void SetCountText()
    {
        GameDay.text = year.ToString() + "��" + month.ToString() + "��" 
            + day.ToString() + "��";
    }

}
