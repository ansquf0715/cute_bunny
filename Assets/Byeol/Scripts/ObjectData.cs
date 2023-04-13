//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////using UnityEngine.EventSystems;

//public class ObjectData : MonoBehaviour //, IPointerClickHandler
//{
//    RaycastHit hit; //raycast에 닿였을 때 hit에 저장
//    static public Rect baseTalkRect;

//    public int id; //오브젝트 id 저장
//    public bool isNpc; //오브젝트가 사물인지 npc인지 저장

//    public NPCManager manager;

//    // Start is called before the first frame update
//    void Start()
//    {
//        manager = FindObjectOfType<NPCManager>();

//        GameObject talkParent = GameObject.FindWithTag("TalkImage");
//        baseTalkRect = talkParent.transform.GetChild(0).GetComponent<RectTransform>().rect;
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    //public void OnPointerClick(PointerEventData eventData)
//    //{
//    //    if (eventData.button == PointerEventData.InputButton.Left)
//    //    {
//    //        Debug.Log("on pointer click 호출");
//    //        manager.ShowText(hit.transform.gameObject);
//    //    }
//    //}

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "Player")
//        {
//            //Debug.Log("틀어오긴 함");
//            if (Physics.Raycast(other.gameObject.transform.position,
//                other.gameObject.transform.forward, out hit, 1000))
//            {
//                //Debug.Log(hit.transform.gameObject);
//                manager.ShowText(hit.transform.gameObject);
//            }
//        }
//    }

//    private void OnTriggerStay(Collider other)
//    {
//        if(other.gameObject.tag == "Player")
//        {
//            //Debug.Log("나는 stay 중");

//            if (Input.GetKeyUp(KeyCode.G))
//            {
//                //Debug.Log("G키 눌림");
//                //Debug.Log("민경이가 시킨거" + hit.transform.gameObject);
//                manager.ShowText(hit.transform.gameObject);
//            }
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.gameObject.tag == "Player")
//            manager.checkImageSetting();
//    }

//}
