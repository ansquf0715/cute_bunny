using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class ObjectData : MonoBehaviour //, IPointerClickHandler
{
    RaycastHit hit; //raycast�� �꿴�� �� hit�� ����
    static public Rect baseTalkRect;

    public int id; //������Ʈ id ����
    public bool isNpc; //������Ʈ�� �繰���� npc���� ����

    public NPCManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<NPCManager>();

        GameObject talkParent = GameObject.FindWithTag("TalkImage");
        baseTalkRect = talkParent.transform.GetChild(0).GetComponent<RectTransform>().rect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        Debug.Log("on pointer click ȣ��");
    //        manager.ShowText(hit.transform.gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Ʋ����� ��");
            if (Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                Debug.Log(hit.transform.gameObject);
                manager.ShowText(hit.transform.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("���� stay ��");

            if (Input.GetKeyUp(KeyCode.G))
            {
                Debug.Log("GŰ ����");
                //Debug.Log("�ΰ��̰� ��Ų��" + hit.transform.gameObject);
                manager.ShowText(hit.transform.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            manager.checkImageSetting();
    }

}
