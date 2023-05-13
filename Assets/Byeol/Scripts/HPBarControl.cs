using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarControl : MonoBehaviour
{
    Player player;

    private Image content;
    public Sprite contentSprite;

    [SerializeField]
    private float lerpSpeed;

    private float maxFill;

    //private GameObject HPObj;
    private float currentFill;

    private bool isChanged;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        content = GetComponent<Image>();
        content.sprite = contentSprite;

        //HPObj = GameObject.FindGameObjectWithTag("HP_fill");
        //content = HPObj.GetComponent();

        setPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChanged == true)
        {
            SetHPUI();
            isChanged = false;
        }
    }

    public void isChange()
    {
        isChanged = true;
        //Debug.Log("is CHange ºÒ·¯Áü");
    }

    void setPlayer()
    {
        maxFill = player.getMaxHealth();
        currentFill = player.getHealth();
    }

    public void SetHPUI()
    {
        currentFill = Player.CurrentHealth / maxFill;
        content.fillAmount = currentFill;
    }

}
