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

    void DayUpdate() //날짜 업데이트
    {
        dTime += Time.deltaTime;
        if (dTime >= 300f) //5분이 지나면 하루가 지나도록 설정
        {
            day++;
            dTime = 0;
        }
    }

    void MonthUpdate() //달 업데이트
    {
        if (day == 31)
        {
            day = 1;
            month++;
        }
    }

    void YearUpdate() //연도 업데이트
    {
        if (month == 13)
        {
            month = 1;
            year++;
        }
    }

    void SetCountText()
    {
        GameDay.text = year.ToString() + "년" + month.ToString() + "월" 
            + day.ToString() + "일";
    }

}
