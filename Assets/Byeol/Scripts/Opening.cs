using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opening : MonoBehaviour
{
    List<Dictionary<string, object>> openingCSV;
    int talkingNum = 0;

    public TextMeshProUGUI text;

    float typingSpeed = 0.3f;
    bool istyping = false;

    public GameObject whiteBack;

    // Start is called before the first frame update
    void Start()
    {
        openingCSV = CSVReader.Read("Opening");

        Invoke("startTalk", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startTalk()
    {
        StartCoroutine(Typing(FindText()));
    }

    string FindText()
    {
        Debug.Log("string" + openingCSV[talkingNum]["message"]);
        return openingCSV[talkingNum]["message"].ToString();
    }

    IEnumerator Typing(string message)
    {
        istyping = true;
        yield return new WaitForSeconds(1f);

        for(int i=0; i<message.Length; i++)
        {
            text.text = message.Substring(0, i + 1);

            yield return new WaitForSeconds(0.05f);
        }

        istyping = false;
        talkingNum++;

        if(talkingNum < openingCSV.Count)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(Typing(FindText()));
        }
        else
        {
            yield return new WaitForSeconds(1f);
            showEye();
        }
    }

    void showEye()
    {
        whiteBack.SetActive(true);

        StartCoroutine(ExpandWhiteBack());
    }

    IEnumerator ExpandWhiteBack()
    {
        float duration = 1f;
        float startWidth = 144f;
        float targetWidth = 1920f;
        float startTime = Time.time;
        float progress;

        while (Time.time - startTime <= duration)
        {
            progress = (Time.time - startTime) / duration;
            float currentWidth = Mathf.Lerp(startWidth, targetWidth, progress);

            RectTransform rt = whiteBack.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(currentWidth, rt.sizeDelta.y);

            yield return null;
        }

        RectTransform rtFinal = whiteBack.GetComponent<RectTransform>();
        rtFinal.sizeDelta = new Vector2(targetWidth, rtFinal.sizeDelta.y);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ExapndWhiteBackHeight());
    }

    IEnumerator ExapndWhiteBackHeight()
    {
        float duration = 1f;
        float startHeight = 13f;
        float targetHeight = 1080f;
        float startTime = Time.time;
        float progress;

        while(Time.time - startTime <= duration)
        {
            progress = (Time.time - startTime) / duration;
            float currentHeight = Mathf.Lerp(startHeight, targetHeight, progress);

            RectTransform rt = whiteBack.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, currentHeight);

            yield return null;
        }

        RectTransform rtFinal = whiteBack.GetComponent<RectTransform>();
        rtFinal.sizeDelta = new Vector2(rtFinal.sizeDelta.x, targetHeight);
    }
}
