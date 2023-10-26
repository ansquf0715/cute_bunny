using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    List<Dictionary<string, object>> openingCSV;
    int talkingNum = 0;

    public TextMeshProUGUI text;

    float typingSpeed = 0.3f;
    bool istyping = false;

    public GameObject blackBack;
    public GameObject whiteBack;
    public Image gameBack;

    float animDuration = 2f;
    float delayDuration = 0.5f;

    int repeatCount = 0;

    public static bool sceneIsChanged = false;

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
            //StartCoroutine(ShrinkText());
            StartCoroutine(HideText());
        }
    }

    IEnumerator HideText()
    {
        float time = 0f;
        Color color = text.color;
        while(time < animDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / animDuration);
            color.a = alpha;
            text.color = color;
            yield return null;
        }

        text.gameObject.SetActive(false);
        showEye();
    }

    IEnumerator ShrinkText()
    {
        float time = 0f;
        while(time<2f)
        {
            time += Time.deltaTime;
            float fontSize = Mathf.Lerp(65f, 0f, time / 2f);
            text.fontSize = fontSize;
            yield return null;
        }

        text.enabled = false;
        showEye();
    }

    void showEye()
    {
        StartCoroutine(Blinking());
    }

    //배경음 페이드인
    

    private IEnumerator Blinking()
    {
        blackBack.SetActive(false);
        whiteBack.SetActive(true);

        Debug.Log("blinking");
        repeatCount = 0; 
        while (repeatCount < 2) 
        {
            float time = 0f;
            while (time < animDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 255f, time / animDuration);
                Color color = gameBack.color;
                color.a = alpha / 255f;
                gameBack.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(delayDuration);

            time = 0f;
            while (time < animDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(255f, 0f, time / animDuration);
                Color color = gameBack.color;
                color.a = alpha / 255f;
                gameBack.color = color;
                yield return null;
            }
            repeatCount++; // repeatCount 증가
        }

        whiteBack.SetActive(false);
        blackBack.SetActive(true);

        yield return new WaitForSeconds(1f);
        StartCoroutine(goToNextScene());
    }

    IEnumerator goToNextScene()
    {
        yield return new WaitForSeconds(1f);
        sceneIsChanged = true;
        Debug.Log("scene fchange" + sceneIsChanged);
        SceneManager.LoadScene("Playing");
    }
}
