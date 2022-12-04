using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; //��ȭ ���� ���� ����
    Dictionary<int, Sprite> portraitData; //��ȭ�ϰ� �ִ� ������Ʈ�� ���� ����

    public Sprite[] portraitSprite; //portrait data�� �ʱ�ȭ ���� sprite�� ����

    // Start is called before the first frame update
    void Start()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        MakeData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeData()
    {
        talkData.Add(1000, new string[] { "�ȳ�?", "�� ���� ó�� �Ա���",
        "���� �� ������", "�����ְڴٰ�?!", "���� ����!", "�ָӴϿ� ���� ������ �־�׾�",
        "Q�� ������ Ȯ���غ�!", "Quest�� ������ ���ƿ�!"});

        //talkData.Add(2000, new string[] { "ó�� ���� ���ε�", "������??" });

        portraitData.Add(1000, portraitSprite[0]);
        //portraitData.Add(2000, portraitSprite[1]);
    }

    public string GetTalk(int id, int talkIndex)
    {

        //Debug.Log("Get Talk ȣ��");
        if (talkIndex == talkData[id].Length)
        {
            //Debug.Log("get talk���� ����dhs talk data null");
            return null;

        }
        else
        {
            Debug.Log("talk Data" + talkData[id][talkIndex]);
            return talkData[id][talkIndex];
        }

    }

    public Sprite GetSprite(int id)
    {
        return portraitData[id];
    }
}
