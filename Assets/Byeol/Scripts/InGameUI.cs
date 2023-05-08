using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using StatePattern;

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

    List<int> randomNumbers = new List<int>();
    List<bool> bossIsShownDay = new List<bool>();
    //bool bossIsShown = false;

    public static List<int> GenerateRandomNumbers(int count)
    {
        //1부터 28까지의 숫자를 리스트에 저장
        //List<int> numbers = Enumerable.Range(1, 28).ToList();
        List<int> numbers = Enumerable.Range(1, 7).ToList();
        List<int> result = new List<int>();

        //count개의 랜덤한 숫자를 고르기 위해 반복
        for(int i=0; i<count; i++)
        {
            //아직 랜덤한 숫자를 고르지 않은 숫자들 중에서 랜덤한 숫자 하나를 선택
            int index = Random.Range(0, numbers.Count);
            int number = numbers[index];

            // 선택한 숫자를 결과 리스트에 추가하고, 숫자 리스트에서 제거
            result.Add(number);
            numbers.RemoveAt(index);
        }

        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameDay = GameObject.Find("DayText").GetComponent<Text>();
        year = 1;
        month = 1;
        day = 1;

        //audio = GameObject.Find("AudioManager");
        //audioSource = audio.GetComponent<AudioSource>();

        selectBossDay();

        for(int i=0; i<randomNumbers.Count; i++)
        {
            bossIsShownDay.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DayUpdate();
        MonthUpdate();
        YearUpdate();
        SetCountText();

        showBoss();

        checkInput();
        menuPageInput();
    }

    void DayUpdate() //날짜 업데이트
    {
        dTime += Time.deltaTime;
        //if (dTime >= 300f) //5분이 지나면 하루가 지나도록 설정
        if (dTime >= 500f) //5분이 지나면 하루가 지나도록 설정
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

    void selectBossDay() //boss 등장하는 날 선택
    {
        randomNumbers = GenerateRandomNumbers(6);
        foreach (int a in randomNumbers)
            Debug.Log("random numbers" + a);
    }

    void showBoss()
    {
        for(int i=0; i<randomNumbers.Count; i++)
        {
            //day가 randomNumbers와 같아지면
            if(day == randomNumbers[i])
            {
                //boss가 아직 안생성됐으면
                if(!bossIsShownDay[i])
                {
                    //Debug.Log("boss is shown" + i);
                    BossControl.toCreateBoss = true;
                    bossIsShownDay[i] = true;
                }
            }
        }
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
