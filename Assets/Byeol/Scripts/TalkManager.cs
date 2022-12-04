using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; //대화 관련 정보 저장
    Dictionary<int, Sprite> portraitData; //대화하고 있는 오브젝트의 사진 저장

    public Sprite[] portraitSprite; //portrait data를 초기화 해줄 sprite를 저장

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
        talkData.Add(1000, new string[] { "안녕?", "이 곳에 처음 왔구나",
        "나를 좀 도와줘", "도와주겠다고?!", "정말 고마워!", "주머니에 관련 내용을 넣어뒀어",
        "Q를 눌러서 확인해봐!", "Quest가 끝나면 돌아와!"});

        //talkData.Add(2000, new string[] { "처음 보는 얼굴인데", "누구야??" });

        portraitData.Add(1000, portraitSprite[0]);
        //portraitData.Add(2000, portraitSprite[1]);
    }

    public string GetTalk(int id, int talkIndex)
    {

        //Debug.Log("Get Talk 호출");
        if (talkIndex == talkData[id].Length)
        {
            //Debug.Log("get talk에서 가져dhs talk data null");
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
