using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using StatePattern;
using TMPro;

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
    public Button forward;
    public Button back;
    public Button left;
    public Button right;

    public GameObject audio;
    AudioSource audioSource;

    List<int> randomNumbers = new List<int>();
    List<bool> bossIsShownDay = new List<bool>();
    //bool bossIsShown = false;

    bool forwardButtonToChange = false;
    bool backButtonToChange = false;
    bool leftButtonToChange = false;
    bool rightButtonToChange = false;

    PlayerMove move;

    KeyCode forwardKey, backKey, leftKey, rightKey;

    public static List<int> GenerateRandomNumbers(int count)
    {
        //1���� 28������ ���ڸ� ����Ʈ�� ����
        //List<int> numbers = Enumerable.Range(1, 28).ToList();
        List<int> numbers = Enumerable.Range(1, 7).ToList();
        List<int> result = new List<int>();

        //count���� ������ ���ڸ� ���� ���� �ݺ�
        for(int i=0; i<count; i++)
        {
            //���� ������ ���ڸ� ���� ���� ���ڵ� �߿��� ������ ���� �ϳ��� ����
            int index = Random.Range(0, numbers.Count);
            int number = numbers[index];

            // ������ ���ڸ� ��� ����Ʈ�� �߰��ϰ�, ���� ����Ʈ���� ����
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
        day = 3;

        audio = GameObject.Find("AudioManager");
        audioSource = audio.GetComponent<AudioSource>();

        selectBossDay();

        for(int i=0; i<randomNumbers.Count; i++)
        {
            bossIsShownDay.Add(false);
        }

        move = FindObjectOfType<PlayerMove>();
        forwardKey = move.vAxisPositiveKey;
        backKey = move.vAxisNegativeKey;
        leftKey = move.hAxisNegativeKey;
        rightKey = move.hAxisPositiveKey;

        Text forwardText = forward.GetComponentInChildren<Text>();
        forwardText.text = forwardKey.ToString();
        Text backText = back.GetComponentInChildren<Text>();
        backText.text = backKey.ToString();
        Text leftText = left.GetComponentInChildren<Text>();
        leftText.text = leftKey.ToString();
        Text rightText = right.GetComponentInChildren<Text>();
        rightText.text = rightKey.ToString();
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

    void DayUpdate() //��¥ ������Ʈ
    {
        dTime += Time.deltaTime;
        //if (dTime >= 300f) //5���� ������ �Ϸ簡 �������� ����
        if (dTime >= 500f) //5���� ������ �Ϸ簡 �������� ����
        {
            day++;
            dTime = 0;
        }
    }

    void MonthUpdate() //�� ������Ʈ
    {
        if (day == 31)
        {
            day = 1;
            month++;
        }
    }

    void YearUpdate() //���� ������Ʈ
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

    void selectBossDay() //boss �����ϴ� �� ����
    {
        randomNumbers = GenerateRandomNumbers(6);
        //foreach (int a in randomNumbers)
        //    Debug.Log("random numbers" + a);
    }

    void showBoss()
    {
        for(int i=0; i<randomNumbers.Count; i++)
        {
            //day�� randomNumbers�� ��������
            if(day == randomNumbers[i])
            {
                //boss�� ���� �Ȼ���������
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

    public void Forward()
    {
        if(!forwardButtonToChange)
        {
            forwardButtonToChange = true;
            StartCoroutine(WaitForKeyInput(MoveDirection.Forward));
        }
    }

    public void Left()
    {
        if(!leftButtonToChange)
        {
            leftButtonToChange = true;
            StartCoroutine(WaitForKeyInput(MoveDirection.Left));
        }
    }

    public void Right()
    {
        if(!rightButtonToChange)
        {
            rightButtonToChange = true;
            StartCoroutine(WaitForKeyInput(MoveDirection.Right));
        }
    }

    public void Back()
    {
        if(!backButtonToChange)
        {
            backButtonToChange = true;
            StartCoroutine(WaitForKeyInput(MoveDirection.Back));
        }
    }

    IEnumerator WaitForKeyInput(MoveDirection direction)
    {
        Text controlText = GetControlTextComponent(direction);
        controlText.text = "";

        while (IsButtonToChangeActive(direction))
        {
            if (Input.anyKeyDown)
            {
                KeyCode pressedKey = GetPressedKey();
                if (pressedKey != KeyCode.None)
                {
                    PlayerMove pmScript = FindObjectOfType<PlayerMove>();
                    if (pmScript != null)
                    {
                        pmScript.SetDirectionKey(pressedKey, direction);

                        if (controlText != null)
                            controlText.text = pressedKey.ToString();
                    }
                }
                SetButtonToChangeActive(direction, false);
            }
            yield return null;
        }
    }

    KeyCode GetPressedKey()
    {
        foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key))
                return key;
        }
        return KeyCode.None;
    }

    bool IsButtonToChangeActive(MoveDirection direction)
    {
        switch(direction)
        {
            case MoveDirection.Forward:
                return forwardButtonToChange;
            case MoveDirection.Back:
                return backButtonToChange;
            case MoveDirection.Left:
                return leftButtonToChange;
            case MoveDirection.Right:
                return rightButtonToChange;
            default:
                return false;
        }
    }

    void SetButtonToChangeActive(MoveDirection direction, bool isActive)
    {
        switch(direction)
        {
            case MoveDirection.Forward:
                forwardButtonToChange = isActive;
                break;
            case MoveDirection.Back:
                backButtonToChange = isActive;
                break;
            case MoveDirection.Left:
                leftButtonToChange = isActive;
                break;
            case MoveDirection.Right:
                rightButtonToChange = isActive;
                break;
            default:
                break;
        }
    }

    Text GetControlTextComponent(MoveDirection direction)
    {
        Button button = GetControlButton(direction);
        if(button != null)
        {
            Text controlText = button.GetComponentInChildren<Text>();
            return controlText;
        }
        return null;
    }

    Button GetControlButton(MoveDirection direction)
    {
        switch(direction)
        {
            case MoveDirection.Forward:
                return forward;
            case MoveDirection.Back:
                return back;
            case MoveDirection.Left:
                return left;
            case MoveDirection.Right:
                return right;
            default:
                return null;
        }
    }
}
