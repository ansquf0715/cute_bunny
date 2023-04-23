using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private Text GameDay;

    private float dTime;
    private int day;
    private int month;
    private int year;

    bool menuPageIsOn = false;
    bool backGroundMusicOn = true;

    public GameObject menuPage;
    public GameObject musicOnOffButton;
    public Image musicOnOffButtonImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Slider musicSlider;

    public GameObject audio;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GameDay = GameObject.Find("DayText").GetComponent<Text>();
        year = 1;
        month = 1;
        day = 1;

        audio = GameObject.Find("AudioManager");
        audioSource = audio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DayUpdate();
        MonthUpdate();
        YearUpdate();
        SetCountText();

        checkInput();
        menuPageInput();
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
        GameDay.text = year.ToString() + "Year " + month.ToString() + "Month " 
            + day.ToString() + "Day ";
    }

    void menuPageInput()
    {
        if(menuPageIsOn == true)
        {
            menuPage.SetActive(true);
            menuPageIsOn = true;
        }
        else
        {
            menuPage.SetActive(false);
            menuPageIsOn = false;
        }
    }

    void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPageIsOn)
                menuPageIsOn = false;
            else
                menuPageIsOn = true;
        }
    }

    public void clickGoToMain()
    {
        SceneManager.LoadScene("GameStart");
        Time.timeScale = 1;
    }

    public void clickRestart()
    {
        SceneManager.LoadScene("Opening");
        Time.timeScale = 1;
    }

    public void clickQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL("http://google.com");
#else
        Application.Quit();
#endif

        Time.timeScale = 1;
    }

    public void ToggleMusic()
    {
        if(backGroundMusicOn)
        {
            backGroundMusicOn = false;
            audioSource.volume = 0f;
            musicOnOffButtonImage.sprite = musicOffSprite;
        }
        else
        {
            backGroundMusicOn = true;
            audioSource.volume = 1f;
            musicOnOffButtonImage.sprite = musicOnSprite;
        }
    }

    public void BackVolume(float volume)
    {
        audioSource.volume = musicSlider.value;
    }
}
