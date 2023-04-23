using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Princess : MonoBehaviour
{
    List<Dictionary<string, object>> princessTalking;
    int talkingNum = 0;

    RaycastHit hit;

    public GameObject npcUI;
    public GameObject backGroundTalking;
    public GameObject npcPortrait;
    public GameObject nameTag;

    Sprite npcPortraitSprite;
    Text talkText;

    string npcName;
    public Sprite princessPortrait;

    bool firstMeet = false;
    bool isStay = false;

    public ParticleSystem particle;

    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        npcName = this.gameObject.name;

        princessTalking = CSVReader.Read("PrincessTalking");
        talkText = backGroundTalking.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStay)
            playerTalkingToNPC();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Physics.Raycast(other.gameObject.transform.position,
                other.gameObject.transform.forward, out hit, 1000))
            {
                isStay = true;
                npcPortrait.GetComponent<Image>().sprite = princessPortrait;
                npcUI.SetActive(true);
                nameTag.SetActive(true);

                if(firstMeet == false) //ó�� �����߸� ��ȭ ����
                {
                    firstMeet = true;

                    if(SellingBox.moneyToEscape == false) //����� money�� ������
                    {
                        //talkingNum = 10;
                        //10������ �� ��������� ���
                        for(int i=0; i<3; i++)
                        {
                            Debug.Log("i" + princessTalking[i]["message"]);
                            talkText.text = " " + princessTalking[i]["message"];
                            talkingNum++;
                            break;
                        }
                    }
                    else //����� money�� ������
                    {
                        for (int i = 4; i < princessTalking.Count; i++)
                        {
                            if (princessTalking[i]["name"].ToString() == npcName)
                            {
                                talkText.text = " " + princessTalking[i]["message"];
                                talkingNum++;
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
                else //ó�� ������ �ƴϸ� ���� ���ؾ��� -> quest page�� ����
                {
                    if(SellingBox.moneyToEscape == true)
                    {
                        if (princessTalking[talkingNum]["name"].ToString() == npcName)
                        {
                            talkText.text = " " + princessTalking[talkingNum]["message"];
                            talkingNum++;
                        }
                    }
                }
            }
        }
    }

    void playerTalkingToNPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(princessTalking[talkingNum]["name"].ToString()==npcName)
            {
                if(SellingBox.moneyToEscape == false) //���� ����
                {
                    if(talkingNum >=3) //�� �� �������� ��ĭ
                    {
                        talkText.text = " " + princessTalking[13]["message"];
                    }
                    else if(talkingNum<3) //�� �� ����������
                    {
                        talkText.text = " " + princessTalking[talkingNum]["message"];
                        talkingNum++;
                    }
                }
                else
                {
                    talkText.text = " " + princessTalking[talkingNum]["message"];
                    talkingNum++;
                }

                if(talkingNum >= 13)
                {
                    npcUI.SetActive(false);
                    nameTag.SetActive(false);

                    ParticleSystem light = Instantiate(particle,
                        new Vector3(42.59f, 2.85f, 17.47f), Quaternion.identity);

                    StartCoroutine(goToNextScene());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isStay = false;
            npcUI.SetActive(false);
            nameTag.SetActive(false);
        }
    }

    IEnumerator goToNextScene()
    {
        yield return new WaitForSeconds(2f);

        panel.SetActive(true);

        //fade in effect
        CanvasGroup panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        float fadeDuration = 2f;
        float elapsedTime = 0f;
        while(elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, (elapsedTime / fadeDuration));
            panelCanvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panelCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Ending");
    }
}
