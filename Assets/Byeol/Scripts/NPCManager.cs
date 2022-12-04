using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    RaycastHit hit; //raycast�� �꿴�� �� hit�� ����

    public TalkManager talkManager;
    //public QuestManager questManager;

    public Text talkText; //��ȭâ�� �ؽ�Ʈ �����

    public GameObject TalkImage; //��ȭâ ���

    public GameObject scanObject; //raycast�� Ȯ���� ������Ʈ�� ����
    public Image portraitImage; //������Ʈ�� �̹���

    //public Text questText;

    //isMove�� �ݸ��� ���ο� ���� ������ ��
    public bool isMove; //�÷��̾ ������ �� �ִ��� ������
    public int talkIndex = 0; //���° ��ȭ�� �̾�������

    // Start is called before the first frame update
    void Start()
    {
        talkManager = FindObjectOfType<TalkManager>();
        //questManager = FindObjectOfType<QuestManager>();

        //questText.text = questManager.CheckQuest();

        //talkText.gameObject.SetActive(false);
        TalkImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText(GameObject scanObj)
    {
        //Debug.Log("Show Text �ҷ���");
        scanObject = scanObj;
        ObjectData objectData = scanObject.GetComponent<ObjectData>();
        OnTalk(objectData.id, objectData.isNpc);

        TalkImage.SetActive(true);
    }



    void OnTalk(int id, bool isNpc)
    {
        //Debug.Log("id" + id);
        //Debug.Log("isNpc" + isNpc);

        //int questTalkIndex = questManager.GetQuestTalkIndex(id);

        string talkData = talkManager.GetTalk(id , talkIndex);
        talkIndex++;
        Debug.Log("talkIndex" + talkIndex);

        if (talkData == null)
        {
            isMove = false;
            talkIndex = 0;
            //questText.text = questManager.CheckQuest(id);

            return;
        }

        if(isNpc)
        {
            TalkImage.SetActive(true);
            talkText.text = talkData;
            portraitImage.sprite = talkManager.GetSprite(id);
            portraitImage.color = new Color(1, 1, 1, 1);
        }

        else
        {

            talkText.text = talkData;
            Color color = portraitImage.color;
            color.a = 0f;
            portraitImage.color = color;
            //portraitImage.color = new Color(1, 1, 1, 0);
        }

        //isMove = true;
        //talkIndex++;
        //Debug.Log("talkIndex" + talkIndex);
    }

    public void checkImageSetting()
    {
        TalkImage.SetActive(false);
    }
}
